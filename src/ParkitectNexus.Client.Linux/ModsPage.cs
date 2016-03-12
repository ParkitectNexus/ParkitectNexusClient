using System;
using ParkitectNexus.Data.Presenter;
using Gtk;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;
using System.Diagnostics;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Tasks;
using ParkitectNexus.Data.Tasks.Prefab;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Web.Models;
using ParkitectNexus.Data.Web.API;
using ParkitectNexus.Data.Assets.Modding;
using System.Linq;
using System.Collections.Generic;

namespace ParkitectNexus.Client.Linux
{
    [System.ComponentModel.ToolboxItem (true)]
    public partial class ModsPage : Gtk.Bin, IPresenter, IPage
    {
        private NodeStore _mods = new NodeStore(typeof(TreeNodeModContainer));
        private IParkitect _parkitect;
        private TreeNodeModContainer _selectedMod;
        private Window _parentwindow;
        private IPresenterFactory _presenterFactory;
        private IQueueableTaskManager _queuableTaskManager;
        //hack to get the latest tag
        private Dictionary<string,string> _versionCache = new Dictionary<string, string>();

        public ModsPage (IAssetUpdatesManager updateManager,IQueueableTaskManager queueableTaskManager,IPresenterFactory presenterFactory,IPresenter parentWindow,IParkitect parkitect)
        {
			this._queuableTaskManager = queueableTaskManager;
            this._presenterFactory = presenterFactory;
            this._parentwindow = (Window)parentWindow;
            this._parkitect = parkitect;
            this.Build ();


            //toggle for checkboxes in tree list
            CellRendererToggle toggle = new CellRendererToggle ();
            toggle.Mode = CellRendererMode.Activatable;
            toggle.Toggled += (o, args) => {
                ((TreeNodeModContainer)_mods.GetNode(new TreePath(args.Path))).IsActive = !((TreeNodeModContainer)_mods.GetNode(new TreePath(args.Path))).IsActive;
            };


            CellRendererText nameRenderText = new CellRendererText ();

            //append the columns to the tree
            listViewMods.AppendColumn ("", toggle, "active", 0);
            listViewMods.AppendColumn ("Name", nameRenderText, "text", 1);
            listViewMods.AppendColumn ("Current",new CellRendererText () , "text", 2);
            listViewMods.AppendColumn ("New", new CellRendererText (), "text", 3);


            //style the name column for the case when the current tag and avalible tag don't match
            listViewMods.Columns [1].SetCellDataFunc (nameRenderText, new TreeCellDataFunc (nameRendering));

            listViewMods.ShowAll ();

            //update selected mod
            listViewMods.NodeSelection.Changed += (sender, e) => {
                Gtk.NodeSelection selection = (Gtk.NodeSelection) sender;
                TreeNodeModContainer mod = (TreeNodeModContainer) selection.SelectedNode;
                if(mod != null)
                {
                    ShowMod(mod.ParkitectMod);
                    _selectedMod = mod;
                }
            };

            listViewMods.NodeStore = _mods;



            //update the tag information
            listViewMods.Model.RowInserted += async (o, args) => {
                var mod = (TreeNodeModContainer)((NodeStore)o).GetNode(args.Path);
                try
                {
                    if (mod.ParkitectMod != null && mod.ParkitectMod.Name != null) {
                        
                        if(!_versionCache.ContainsKey(mod.Name))
                        {

                            mod.AvaliableVersion = await updateManager.GetLatestVersionName(mod.ParkitectMod);
                            _versionCache.Add(mod.Name,mod.AvaliableVersion);
                        }
                        else
                        {
                            mod.AvaliableVersion =  _versionCache[mod.Name];

                        }

                        listViewMods.QueueDraw();   
                    }
                }
                catch 
                {
                }
            };

            this.HideModInfo ();
            this.UpdateModList ();

            _queuableTaskManager.TaskAdded += (object sender, QueueableTaskEventArgs e) =>
            {
                       
                e.Task.StatusChanged += (object s, EventArgs e_1) => 
                {
                    if (((IQueueableTask)s) is InstallAssetTask  && ((IQueueableTask)s).Status == TaskStatus.Finished)
                    {
                        Gtk.Application.Invoke(delegate
                        {
                            UpdateModList();
                        });
                    }
                };
            };
        }

        public void OnOpen()
        {
        }

        public void OnClose()
        {
        }

        /// <summary>
        /// Updates the mod list.
        /// </summary>
        private void UpdateModList()
        {
            _mods.Clear ();
            foreach (IModAsset mod in _parkitect.Assets[AssetType.Mod].OfType<ModAsset>().ToArray()) {
                _mods.AddNode (new TreeNodeModContainer (mod));
            }
        }


        /// <summary>
        /// display the specific mod
        /// </summary>
        private void ShowMod(IModAsset mod)
        {
            lblModName.Text = mod.Name;
            lblModVersion.Text = mod.Information.CompilerVersion;
            lblDevelopmentStatus.Visible = mod.Information.IsDevelopment;

            lblViewOnParkitectNexusWebsite.ModifyFg (StateType.Normal, new Gdk.Color (0, 0, 255));
            lblViewOnParkitectNexusWebsite.Text = "View on ParkitectNexus";
            lblViewOnParkitectNexusWebsite.UseUnderline = true;
            lblViewOnParkitectNexusWebsite.Pattern = "______________________";

            btnUpdate.Sensitive = !mod.Information.IsDevelopment;// && !string.IsNullOrWhiteSpace(mod.Repository);
            btnUninstall.Sensitive = !mod.Information.IsDevelopment;
            btnRecompile.Sensitive = true;

        }


        /// <summary>
        /// Hides the mod info.
        /// </summary>
        private void HideModInfo()
        {
            lblModName.Text = "-";
            lblModVersion.Text = "-";
            lblViewOnParkitectNexusWebsite.Text = "-";
            lblViewOnParkitectNexusWebsite.ModifyFg (StateType.Normal, new Gdk.Color (0, 0, 0));

            lblDevelopmentStatus.Visible = false;
            btnUpdate.Sensitive = false;
            btnUninstall.Sensitive = false;
            btnRecompile.Sensitive = false;
        }

        /// <summary>
        /// update tag styling
        /// </summary>
        private void nameRendering (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
        {
            string currentVersion = (string)model.GetValue (iter, 2);
            string avaliableVersion = (string)model.GetValue (iter, 3);

            if (currentVersion!= avaliableVersion && avaliableVersion != "-") {
                (cell as Gtk.CellRendererText).Foreground = "Red";
            } else {
                (cell as Gtk.CellRendererText).Foreground = "Black";
            }
        }

        protected void InstallMod (object sender, EventArgs e)
        {
            ModUri installMod = _presenterFactory.InstantiatePresenter<ModUri> ();
            installMod.Run ();
            installMod.Destroy ();
		
        }

     

        protected void UninstallMod (object sender, EventArgs e)
        {
            MessageDialog errorDialog = new MessageDialog (_parentwindow, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.YesNo, "Are you sure wish to delete "+ _selectedMod.ParkitectMod.Name +"?");
            if(errorDialog.Run() == (int)ResponseType.Yes)
            {
                if (_selectedMod == null) return;
                _mods.RemoveNode (_selectedMod);
                _parkitect.Assets.DeleteAsset(_selectedMod.ParkitectMod);
            }
            errorDialog.Destroy();
            HideModInfo ();
            UpdateModList ();
        }

        protected void VistModWebsite (object o, Gtk.ButtonPressEventArgs args)
        {
            if (_selectedMod == null) return;
              Process.Start($"https://client.parkitectnexus.com/redirect/{_selectedMod.ParkitectMod.Repository}");
           
        }
           

        protected void Recompile (object sender, EventArgs e)
        {
            _queuableTaskManager.With(_selectedMod.ParkitectMod).Add<CompileModTask>();
        }

        protected void Update (object sender, EventArgs e)
        {
            _queuableTaskManager.With(_selectedMod.ParkitectMod).Add<UpdateModTask>();
        }
    }
}

