using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MetroFramework;
using MetroFramework.Controls;
using ParkitectNexus.AssetMagic.Readers;
using ParkitectNexus.Client.Windows.SliderPanels;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Client.Windows.TabPages
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

        #region Overrides of LoadableTilesTabPage

        protected override void ClearTiles()
        {
            foreach (var tile in Controls.OfType<MetroTile>().ToArray())
            {
                Controls.Remove(tile);
                tile.TileImage.Dispose();
            }
        }

        #endregion

        protected override Task<IEnumerable<MetroTile>> LoadTiles(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var tiles = new List<MetroTile>();
                var files = Directory.GetFiles(_parkitect.Paths.GetPathInSavesFolder("Saves/Savegames", true), "*.txt");
                var current = 0;
                foreach (var file in files)
                {
                    cancellationToken.ThrowIfCancellationRequested();


                    var r = new SavegameReader();
                    using (var stream = File.OpenRead(file))
                    {
                        var data = r.Deserialize(stream);
                        var name = data.Header.Name;

                        if (name != null)
                        {
                            var tileImage = new Bitmap(data.Screenshot, 100, 100);

                            var tile = new MetroTile
                            {
                                Text = name,
                                TextAlign = ContentAlignment.BottomCenter,
                                Style = MetroColorStyle.Default,
                                TileImage = tileImage,
                                UseTileImage = true,
                                TileImageAlign = ContentAlignment.MiddleCenter
                            };

                            tile.Click +=
                                (sender, args) =>
                                {
                                    (FindForm() as MainForm)?.SpawnSliderPanel(new SavegameSliderPanel(file, data));
                                };
                            tiles.Add(tile);
                        }
                    }
                    UpdateLoadingProgress((current++*100)/files.Length);
                }
                return (IEnumerable<MetroTile>) tiles;
            }, cancellationToken);
        }

        #endregion
    }
}
