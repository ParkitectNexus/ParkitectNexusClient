// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ParkitectNexus.Client.Base.Main;
using ParkitectNexus.Client.Base.Tiles;
using ParkitectNexus.Client.Base.Utilities;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;
using Xwt;
using Xwt.Drawing;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client.Base.Pages
{
    public class AssetsPageView : LoadableDataTileView
    {
        private readonly IParkitect _parkitect;
        private readonly AssetType _type;

        public AssetsPageView(IParkitect parkitect, AssetType type, IPresenter parent, string displayName)
            : base(displayName)
        {
            if (!(parent is MainView))
                throw new ArgumentException("parent must be MainView", nameof(parent));

            _parkitect = parkitect;
            _type = type;
            MainView = (MainView) parent;

            parkitect.Assets.AssetAdded += Assets_AssetAdded;
            parkitect.Assets.AssetRemoved += Assets_AssetRemoved;
        }

        public MainView MainView { get; }

        private void Assets_AssetRemoved(object sender, AssetEventArgs e)
        {
            if (e.Asset.Type == _type)
                RefreshTiles();
        }

        private void Assets_AssetAdded(object sender, AssetEventArgs e)
        {
            if (e.Asset.Type == _type)
                RefreshTiles();
        }

        protected virtual void PopulateViewBoxWithTitle(VBox vBox, IAsset asset)
        {
            var title = new Label(asset.Name)
            {
                Font = Font.SystemFont.WithSize(17.5).WithWeight(FontWeight.Bold)
            };
            vBox.PackStart(title);
        }

        protected virtual void PopulateViewBoxWithImage(VBox vBox, IAsset asset)
        {
            try
            {
            var image = asset.GetImage();
            if (image != null)
            {
                var imageView = new ImageView(image.ToXwtImage().WithSize(250));
                vBox.PackStart(imageView);
            }
            }
            catch(Exception e)
            {
                ObjectFactory.GetInstance<ILogger>().WriteException(e);
            }
        }

        protected virtual void PopulateViewBoxWithButtons(VBox vBox, IAsset asset)
        {
            var deleteButton = new Button("Delete");
            deleteButton.Clicked += (sender, args) =>
            {
                if (
                    MessageDialog.AskQuestion(
                        $"Are you sure you wish to delete \"{asset.Name}\"? This action cannot be undone!", 0,
                        Command.No,
                        Command.Yes) == Command.Yes)
                {
                    _parkitect.Assets.DeleteAsset(asset);
                    MainView.ShowSidebarWidget(null, null);
                }
            };
            vBox.PackStart(deleteButton);
        }

        protected virtual VBox CreateViewBox(IAsset asset)
        {
            var vBox = new VBox();
            PopulateViewBoxWithTitle(vBox, asset);
            PopulateViewBoxWithImage(vBox, asset);
            PopulateViewBoxWithButtons(vBox, asset);
            return vBox;
        }

        #region Overrides of LoadableDataTileView

        protected override Task<IEnumerable<Tile>> LoadTiles(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var tiles = new List<Tile>();

                if (_parkitect?.Assets == null)
                {
                    return new Tile[0] as IEnumerable<Tile>;
                }

                foreach (var asset in _parkitect.Assets[_type])
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    System.Drawing.Image image = null;
                    try
                    {
                        image = asset.GetImage();
                    }
                    catch(Exception e)
                    {
                        ObjectFactory.GetInstance<ILogger>().WriteException(e);
                    }
                    var tile = new Tile(image, asset.Name,
                        () => { MainView.ShowSidebarWidget(asset.Type.ToString(), CreateViewBox(asset)); });
                    tiles.Add(tile);
                }
                return tiles;
            }, cancellationToken);
        }

        #endregion
    }
}
