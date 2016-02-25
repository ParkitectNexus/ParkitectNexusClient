// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MetroFramework;
using MetroFramework.Controls;
using ParkitectNexus.Client.Windows.Controls.SliderPanels;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client.Windows.Controls.TabPages
{
    public class BlueprintsTabPage : LoadableTilesTabPage
    {
        private readonly IParkitect _parkitect;
        private readonly IAssetUpdatesManager _assetUpdatesManager;

        public BlueprintsTabPage(IParkitect parkitect, IAssetUpdatesManager assetUpdatesManager)
        {
            _parkitect = parkitect;
            _assetUpdatesManager = assetUpdatesManager;

            Text = "Blueprints";
        }

        #region Overrides of LoadableTilesTabPage

        protected override bool ReloadOnEnter { get; } = true;

        protected override void ClearTiles()
        {
            foreach (var tile in Controls.OfType<MetroTile>().ToArray())
            {
                Controls.Remove(tile);
                tile.TileImage.Dispose();
            }
        }

        protected override Task<IEnumerable<MetroTile>> LoadTiles(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var tiles = new List<MetroTile>();

                var current = 0;
                var fileCount = _parkitect.Assets.GetAssetCount(AssetType.Blueprint);
                foreach (var bp in _parkitect.Assets[AssetType.Blueprint])
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (bp.Name != null)
                    {
                        var tile = new MetroTile
                        {
                            Text = bp.Name,
                            TextAlign = ContentAlignment.BottomCenter,
                            Style = MetroColorStyle.Default,
                            TileImage = ImageUtility.ResizeImage(bp.GetImage(), 100, 100),
                            UseTileImage = true,
                            TileImageAlign = ContentAlignment.MiddleCenter
                        };

                        tile.Click +=
                            (sender, args) =>
                                (FindForm() as MainForm)?.SpawnSliderPanel(new BlueprintSliderPanel(bp));
                        tiles.Add(tile);
                    }

                    UpdateLoadingProgress((current++*100)/fileCount);
                }
                return (IEnumerable<MetroTile>) tiles;
            }, cancellationToken);
        }

        #endregion
    }
}
