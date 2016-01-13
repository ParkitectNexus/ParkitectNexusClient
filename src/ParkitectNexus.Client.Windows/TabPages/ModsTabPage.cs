using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MetroFramework;
using MetroFramework.Controls;
using ParkitectNexus.Client.Windows.SliderPanels;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Client.Windows.TabPages
{
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
                        Style = MetroColorStyle.Default
                        //TileImage = tileImage,
                        //UseTileImage = true,
                        //TileImageAlign = ContentAlignment.MiddleCenter,
                    };

                    tile.Click +=
                        (sender, args) => { (FindForm() as MainForm)?.SpawnSliderPanel(new ModSliderPanel(mod)); };
                    tiles.Add(tile);


                    UpdateLoadingProgress((current++*100)/mods.Length);
                }
                return (IEnumerable<MetroTile>) tiles;
            }, cancellationToken);
        }

        #endregion
    }
}
