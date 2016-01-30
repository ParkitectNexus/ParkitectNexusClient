using System;
using Gtk;
using ParkitectNexus.Data.Game;
using Gdk;
using System.IO;
using System.Drawing.Imaging;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Assets;

namespace ParkitectNexus.Client.Linux
{
    [System.ComponentModel.ToolboxItem (true)]
    public partial class SavegamePage : Gtk.Bin, IPresenter, IPage
    {

        const int NAME = 0;
        const int IMAGE = 1;
        const int PARKITECT_ASSET = 2;
        private ListStore _blueprintListStore = new ListStore(typeof(string),typeof(Pixbuf),typeof(IAsset));
		private IAsset _selectedAsset;
		private IParkitect _parkitect;
        private Pixbuf _selectedAssetImage;
        private int _sizeChange = 0;

        public SavegamePage (IParkitect parkitect)
        {
            _parkitect = parkitect;
            this.Build();

            //Pango.FontDescription fontdesc = Pango.FontDescription.FromString("Arial");
            //fontdesc.Size = 25;
            //blueprintName.ModifyFont(fontdesc);

            savegames.Model = _blueprintListStore;
            _blueprintListStore.SetSortColumnId(NAME, SortType.Ascending);

            savegames.TextColumn = NAME;
            savegames.PixbufColumn = IMAGE;

            savegames.SelectionChanged += Savegames_SelectionChanged;;
            GLib.Timeout.Add(100, new GLib.TimeoutHandler(ImageUpdate));
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
                        savegameImage.Pixbuf = _selectedAssetImage.ScaleSimple (lwPaneSize, lwPaneSize, InterpType.Hyper);
                        savegameImage.SetSizeRequest (lwPaneSize, lwPaneSize);
                        savegameImage.QueueDraw ();
                    } else {
                        savegameImage.Pixbuf = _selectedAssetImage.ScaleSimple (lhPaneSize, lhPaneSize, InterpType.Hyper);
                        savegameImage.SetSizeRequest (lhPaneSize, lhPaneSize);
                        savegameImage.QueueDraw ();
                    }
                }

            }

            return true;
        }


        void Savegames_SelectionChanged (object sender, EventArgs e)
        {
            if (savegames.SelectedItems.Length >= 1)
            {
                TreeIter iter;
                _blueprintListStore.GetIter(out iter, savegames.SelectedItems[0]);

				IAsset asset = (IAsset)_blueprintListStore.GetValue(iter, PARKITECT_ASSET);

                _selectedAsset = asset;
                savegameName.Text = _selectedAsset.Name;
                using (MemoryStream stream = new MemoryStream ()) {
					_selectedAsset.GetImage().Result.Save (stream, ImageFormat.Png);
                    stream.Position = 0;
                    _selectedAssetImage = new Pixbuf(stream);
                }

                _sizeChange = -1;
            }
        }

        public void UpdateListStore()
        {
            _blueprintListStore.Clear();
			foreach (var savegame in _parkitect.LocalAssets.GetAssets(AssetType.Savegame))
            {
					using (MemoryStream stream = new MemoryStream ()) {
                    
						savegame.GetImage ().Result.Save (stream, ImageFormat.Png);
						stream.Position = 0;
						Pixbuf pixbuf = new Pixbuf (stream);
						_blueprintListStore.AppendValues (savegame.Name, pixbuf, savegame);
					}
            }
        }

        public void OnOpen()
        {
            UpdateListStore();
        }

        public void OnClose()
        {
        }
    }
}

