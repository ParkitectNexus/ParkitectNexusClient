// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MetroFramework;
using MetroFramework.Controls;
using ParkitectNexus.Client.Windows.Controls.SliderPanels;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client.Windows.Controls.TabPages
{
    public abstract class AssetsTabPage : LoadableTilesTabPage
    {
        private readonly IParkitect _parkitect;
        private readonly AssetType _type;

        protected AssetsTabPage(IParkitect parkitect, AssetType type)
        {
            _parkitect = parkitect;
            _type = type;

            parkitect.Assets.AssetRemoved += LocalAssetRepository_AssetRemoved;
        }

        private void LocalAssetRepository_AssetRemoved(object sender, AssetEventArgs e)
        {
            var tile = Controls.OfType<MetroTile>().FirstOrDefault(t => Equals(t.Tag, e.Asset));

            if (tile == null) return;
            Controls.Remove(tile);
            RepositionTiles();
        }

        protected virtual MetroTile CreateTileForAsset(IAsset asset)
        {
            return new MetroTile
            {
                Text = asset.Name,
                TextAlign = ContentAlignment.BottomCenter,
                Style = MetroColorStyle.Default,
                TileImage = ImageUtility.ResizeImage(asset.GetImage(), 100, 100),
                UseTileImage = true,
                TileImageAlign = ContentAlignment.MiddleCenter,
                Tag = asset
            };
        }

        protected virtual void AddClickHandlerToTile(MetroTile tile, IAsset asset)
        {
            tile.Click +=
                (sender, args) =>
                {
                    var slider = ObjectFactory.Container.With("asset")
                        .EqualTo(asset)
                        .GetInstance<AssetSliderPanel>();

                    (FindForm() as MainForm)?.SpawnSliderPanel(slider);
                };
        }

        #region Overrides of LoadableTilesTabPage

        protected override bool ReloadOnEnter { get; } = true;

        protected override void ClearTiles()
        {
            foreach (var tile in Controls.OfType<MetroTile>().ToArray())
            {
                Controls.Remove(tile);
                tile.TileImage?.Dispose();
            }
        }

        protected override Task<IEnumerable<MetroTile>> LoadTiles(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var tiles = new List<MetroTile>();

                var current = 0;
                var fileCount = _parkitect.Assets.GetAssetCount(_type);
                foreach (var asset in _parkitect.Assets[_type])
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var tile = CreateTileForAsset(asset);
                    AddClickHandlerToTile(tile, asset);
                    tiles.Add(tile);

                    UpdateLoadingProgress((current++*100)/fileCount);
                }
                return (IEnumerable<MetroTile>) tiles;
            }, cancellationToken);
        }

        #endregion
    }
}
