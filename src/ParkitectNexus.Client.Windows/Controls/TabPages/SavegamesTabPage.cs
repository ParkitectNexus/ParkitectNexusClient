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
    public class SavegamesTabPage : LoadableTilesTabPage
    {
        private readonly IParkitect _parkitect;

        public SavegamesTabPage(IParkitect parkitect)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            _parkitect = parkitect;

            Text = "Savegames";
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
                var fileCount = _parkitect.Assets.GetAssetCount(AssetType.Savegame);
                foreach (var sg in _parkitect.Assets[AssetType.Savegame])
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var name = sg.Name;

                    if (name != null)
                    {
                        var tile = new MetroTile
                        {
                            Text = name,
                            TextAlign = ContentAlignment.BottomCenter,
                            Style = MetroColorStyle.Default,
                            TileImage = ImageUtility.ResizeImage(sg.GetImage(), 100, 100),
                            UseTileImage = true,
                            TileImageAlign = ContentAlignment.MiddleCenter
                        };

                        tile.Click +=
                            (sender, args) =>
                                (FindForm() as MainForm)?.SpawnSliderPanel(new SavegameSliderPanel(sg));
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