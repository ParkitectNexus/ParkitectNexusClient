using System;
using Gtk;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;
using Gdk;
using System.IO;
using System.Drawing.Imaging;

namespace ParkitectNexus.Client.Linux
{
  
    [System.ComponentModel.ToolboxItem (true)]
    public partial class BlueprintPage : Gtk.Bin, IPresenter
    {
        const int NAME = 0;
        const int IMAGE = 1;
        const int PARKITECT_ASSET = 2;
        private ListStore _blueprintListStore = new ListStore(typeof(string),typeof(Pixbuf),typeof(IParkitectAsset));
        private IParkitect _parkitect;
        private IParkitectAsset _selectedAsset;
        private Pixbuf _selectedAssetImage;
        private int _sizeChange = 0;
        public BlueprintPage (IParkitect parkitect)
        {
            _parkitect = parkitect;
            this.Build();

            blueprints.Model = _blueprintListStore;
            UpdateListStore();
            _blueprintListStore.SetSortColumnId(NAME, SortType.Ascending);

            blueprints.TextColumn = NAME;
            blueprints.PixbufColumn = IMAGE;
            
            blueprints.SelectionChanged += Blueprints_SelectionChanged;
            GLib.Timeout.Add(100, new GLib.TimeoutHandler(ImageUpdate));

        }

        private void Blueprints_SelectionChanged(object sender, EventArgs e)
        {
            if (blueprints.SelectedItems.Length >= 1)
            {
                TreeIter iter;
                _blueprintListStore.GetIter(out iter, blueprints.SelectedItems[0]);

                IParkitectAsset asset = (IParkitectAsset)_blueprintListStore.GetValue(iter, PARKITECT_ASSET);

                _selectedAsset = asset;
                blueprintName.Text = _selectedAsset.Name;
                _selectedAssetImage = new Pixbuf(_selectedAsset.Open());
                _sizeChange = -1;
            }
        }


        private bool ImageUpdate()
        {
            if (_selectedAsset != null)
            {
                
                int lwPaneSize = (BlueprintPane.MaxPosition - BlueprintPane.Position)-20;
                if (_sizeChange != lwPaneSize) {
                    _sizeChange = lwPaneSize;
                    int lhPaneSize = (int)(BlueprintPane.Allocation.Height * .8f);
                    if (lwPaneSize < lhPaneSize) {
                        blueprintImage.Pixbuf = _selectedAssetImage.ScaleSimple (lwPaneSize, lwPaneSize, InterpType.Hyper);
                        blueprintImage.SetSizeRequest (lwPaneSize, lwPaneSize);
                        blueprintImage.QueueDraw ();
                    } else {
                        blueprintImage.Pixbuf = _selectedAssetImage.ScaleSimple (lhPaneSize, lhPaneSize, InterpType.Hyper);
                        blueprintImage.SetSizeRequest (lhPaneSize, lhPaneSize);
                        blueprintImage.QueueDraw ();
                    }
                }

            }
           
            return true;
        }


        public void UpdateListStore()
        {
            foreach (var blueprint in _parkitect.GetAssets(ParkitectAssetType.Blueprint))
            {

                using (MemoryStream stream = new MemoryStream())
                {

                    blueprint.Thumbnail.Save(stream, ImageFormat.Png);
                    stream.Position = 0;
                    Pixbuf pixbuf = new Pixbuf(stream);
                    _blueprintListStore.AppendValues(blueprint.Name, pixbuf,blueprint);
                }
            }
        }

        
    
    }
}

