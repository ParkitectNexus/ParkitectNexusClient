using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MetroFramework;
using MetroFramework.Controls;
using ParkitectNexus.AssetMagic.Readers;
using ParkitectNexus.Client.Windows.SliderPanels;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Client.Windows.TabPages
{
    public class BlueprintsTabPage : LoadableTilesTabPage
    {
        private readonly IParkitect _parkitect;

        public BlueprintsTabPage(IParkitect parkitect)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            _parkitect = parkitect;

            Text = "Blueprints";
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
                var files = Directory.GetFiles(_parkitect.Paths.GetPathInSavesFolder("Saves/Blueprints", true), "*.png");
                var current = 0;
                foreach (var file in files)
                {
                    cancellationToken.ThrowIfCancellationRequested();


                    var r = new BlueprintReader();
                    using (var bitmap = (Bitmap)Image.FromFile(file))
                    {
                        var data = r.Read(bitmap);
                        var name = data.Header.Name;

                        if (name != null)
                        {
                            var tileImage = new Bitmap(bitmap, 100, 100);

                            var tile = new MetroTile
                            {
                                Text = name,
                                TextAlign = ContentAlignment.BottomCenter,
                                Style = MetroColorStyle.Default,
                                TileImage = tileImage,
                                UseTileImage = true,
                                TileImageAlign = ContentAlignment.MiddleCenter,
                            };

                            tile.Click += (sender, args) => { (FindForm() as MainForm)?.SpawnSliderPanel(new BlueprintSliderPanel(file, data)); };
                            tiles.Add(tile);
                        }
                    }

                    UpdateLoadingProgress((current++ * 100) / files.Length);
                }
                return (IEnumerable<MetroTile>)tiles;
            }, cancellationToken);
        }

        #endregion
    }
    public class ModsTabPage : LoadableTilesTabPage
    {
        private readonly IParkitect _parkitect;

        public ModsTabPage(IParkitect parkitect)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            _parkitect = parkitect;

            Text = "Mods";
        }

        #region Overrides of LoadableTilesTabPage

        protected override bool ReloadOnEnter { get; } = true;

        protected override Task<IEnumerable<MetroTile>> LoadTiles(CancellationToken cancellationToken)
        {

            return Task.Run(() =>
            {
                var tiles = new List<MetroTile>();
                var mods = _parkitect.InstalledMods.ToArray();
                var current = 0;

                foreach (var mod in mods)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var tile = new MetroTile
                    {
                        Text = mod.Name,
                        TextAlign = ContentAlignment.MiddleCenter,
                        //TextAlign = ContentAlignment.BottomCenter,
                        Style = MetroColorStyle.Default,
                        //TileImage = tileImage,
                        //UseTileImage = true,
                        //TileImageAlign = ContentAlignment.MiddleCenter,
                    };

                    tile.Click +=
                        (sender, args) => { (FindForm() as MainForm)?.SpawnSliderPanel(new ModSliderPanel(mod)); };
                    tiles.Add(tile);


                    UpdateLoadingProgress((current++*100)/mods.Length);
                }
                return (IEnumerable<MetroTile>)tiles;
            }, cancellationToken);
        }

        #endregion
    }
}
