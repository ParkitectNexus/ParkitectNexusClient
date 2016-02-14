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
using ParkitectNexus.Data;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Tasks;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client.Windows.Controls.TabPages
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
                var mods = _parkitect.Assets[AssetType.Mod].OfType<ModAsset>().ToArray();
                var current = 0;

                foreach (var mod in mods)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var tile = new MetroTile
                    {
                        Text = mod.Name,
                        TextAlign = ContentAlignment.BottomCenter,
                        Style = MetroColorStyle.Default,
                        TileImage = ImageUtility.ResizeImage(mod.GetImage(), 100, 100),
                        UseTileImage = true,
                        TileImageAlign = ContentAlignment.MiddleCenter
                    };

                    tile.Click +=
                        (sender, args) => { (FindForm() as MainForm)?.SpawnSliderPanel(new ModSliderPanel(ObjectFactory.GetInstance<IQueueableTaskManager>(), mod)); };
                    tiles.Add(tile);


                    UpdateLoadingProgress((current++*100)/mods.Length);
                }
                return (IEnumerable<MetroTile>) tiles;
            }, cancellationToken);
        }

        #endregion
    }
}
