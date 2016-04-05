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

namespace ParkitectNexus.Client.Base.Pages
{
    public class AssetsPageView : LoadableDataTileView
    {
        private readonly MainView _mainView;
        private readonly IParkitect _parkitect;
        private readonly AssetType _type;

        public AssetsPageView(IParkitect parkitect, AssetType type, IPresenter parent)
        {
            if (!(parent is MainView))
                throw new ArgumentException("parent must be MainView", nameof(parent));

            _parkitect = parkitect;
            _type = type;
            _mainView = (MainView) parent;
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
            var image = asset.GetImage();
            if (image != null)
            {
                var imageView = new ImageView(image.ToXwtImage().WithSize(250));
                vBox.PackStart(imageView);
            }
        }

        protected virtual void PopulateViewBoxWithButtons(VBox vBox, IAsset asset)
        {
            var deleteButton = new Button("Delete");
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

                    var tile = new Tile(asset.GetImage(), asset.Name,
                        () => { _mainView.ShowSidebarWidget(asset.Type.ToString(), CreateViewBox(asset)); });
                    tiles.Add(tile);
                }
                return tiles;
            }, cancellationToken);
        }

        #endregion
    }
}
