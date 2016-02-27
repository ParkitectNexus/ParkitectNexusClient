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
using ParkitectNexus.Client.Windows.Properties;
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
        private readonly IAssetUpdatesManager _assetUpdatesManager;

        public ModsTabPage(IParkitect parkitect, IAssetUpdatesManager assetUpdatesManager)
        {
            _parkitect = parkitect;
            _assetUpdatesManager = assetUpdatesManager;

            assetUpdatesManager.UpdateFound += AssetUpdatesManager_UpdateFound;
            Text = "Mods";
        }

        private void AssetUpdatesManager_UpdateFound(object sender, AssetEventArgs e)
        {
            DrawUpdateAvailable(Controls.OfType<MetroTile>().FirstOrDefault(t => t.Tag == e.Asset));
        }

        private void DrawUpdateAvailable(MetroTile tile)
        {
            if (tile == null)
                return;

            if (tile.Image == null)
                tile.Image = Resources.update_available;

            using (var g = Graphics.FromImage(tile.Image))
            {
                g.DrawImage(Resources.update_available, 0, 0);
            }
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
                        TileImageAlign = ContentAlignment.MiddleCenter,
                        Tag = mod
                    };

                    if(_assetUpdatesManager.HasChecked && _assetUpdatesManager.IsUpdateAvailableInMemory(mod))
                        DrawUpdateAvailable(tile);

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
