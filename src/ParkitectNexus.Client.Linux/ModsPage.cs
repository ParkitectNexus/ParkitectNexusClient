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
        private ILogger _logger;
		private IQueueableTaskManager _queuableTaskManager;
		private IRemoteAssetRepository _assetRepository;
		private IParkitectNexusAPI _nexusAPI;
	
		public ModsPage (ILogger logger,IParkitectNexusAPI nexusAPI,IRemoteAssetRepository assetRepository,IQueueableTaskManager queueableTaskManager,IPresenterFactory presenterFactory,IPresenter parentWindow,IParkitect parkitect)
        {
			this._nexusAPI = nexusAPI;
			this._assetRepository = assetRepository;
			this._queuableTaskManager = queueableTaskManager;
            this._logger = logger;
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
                try{
                    var mod = (TreeNodeModContainer)((NodeStore)o).GetNode(args.Path);
                    if (mod.ParkitectMod != null && mod.ParkitectMod.Name != null) {
                    //    var url = new ParkitectNexusUrl (mod.ParkitectMod.Name, ParkitectAssetType.Mod, mod.ParkitectMod.Repository);
                    //    var info = await _parkitectAssetRepository.ResolveDownloadInfo (url); 
                      //  mod.AvaliableVersion = info.Tag;
                        listViewMods.QueueDraw();   
                    }
                }
                catch (Exception)
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

            btnCheckUpdate.Sensitive = !mod.Information.IsDevelopment;// && !string.IsNullOrWhiteSpace(mod.Repository);
            btnUninstall.Sensitive = !mod.Information.IsDevelopment;
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
            btnCheckUpdate.Sensitive = false;
            btnUninstall.Sensitive = false;
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

        protected void CheckForUpdatesForMod (object sender, EventArgs e)
        {
            if (_selectedMod == null) return;

            //try
           // {


                   /*NexusUrl nexusURL;
                 NexusUrl.TryParse (_selectedMod.ParkitectMod.Repository, out nexusURL);
				var task = new InstallAssetTask (_parkitect, _website, _assetRepository);
				task.Data = nexusURL.Data;
				_queuableTaskManager.Add (task);*/

				//var installAssetTask = new InstallAssetTask(_parkitect,_nexusWebsite,_assetRepository);
				//var url = new ParkitectNexusUrl(_selectedMod.ParkitectMod.Name, _selectedMod.ParkitectMod.Repository);
				//installAssetTask.Data = url.Data;
				//installAssetTask.Data = _nexusAPI.
				//this._queuableTaskManager.Add(installAssetTask);
               // var info =  _parkitectAssetRepository.ResolveDownloadInfo(url).Result;

               /* if (info.Tag == _selectedMod.ParkitectMod.Tag)
                {
                    MessageDialog errorDialog = new MessageDialog (_parentwindow, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, _selectedMod.ParkitectMod.Name +" is already up to date!");
                    errorDialog.Run();
                    errorDialog.Destroy();
                }
                else
                {
                    var installDialog = new ModInstallDialog(url, this, _logger, _parkitect, _parkitectAssetRepository);
                    installDialog.Run();
                    installDialog.Destroy();
                    //update the tag info and update the mod info
                    _selectedMod.AvaliableVersion = info.Tag;
                    _selectedMod.ParkitectMod.Tag = info.Tag;
                    ShowMod(_selectedMod.ParkitectMod);

                    listViewMods.QueueDraw();
                }*/
         /*   }
            catch (Exception)
            {
                Gtk.MessageDialog errorDialog = new MessageDialog (_parentwindow, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, "Failed to check for updates. Please try again later.");
                errorDialog.Run();
                errorDialog.Destroy();
            }*/
        }

        protected void UninstallMod (object sender, EventArgs e)
        {
            if (_selectedMod == null) return;
            _mods.RemoveNode (_selectedMod);
            //_selectedMod.ParkitectMod.Delete();
            HideModInfo ();
            UpdateModList ();
        }

        protected void VistModWebsite (object o, Gtk.ButtonPressEventArgs args)
        {
           // if (_selectedMod == null) return;
          //  Process.Start($"https://client.parkitectnexus.com/redirect/{_selectedMod.ParkitectMod.Repository}");
        }

        public void OnOpen()
        {
        }

        public void OnClose()
        {
        }
    }
}

