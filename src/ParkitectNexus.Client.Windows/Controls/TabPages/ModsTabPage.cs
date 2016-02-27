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
    public class ModsTabPage : AssetsTabPage
    {
        private readonly IParkitect _parkitect;
        private readonly IAssetUpdatesManager _assetUpdatesManager;

        public ModsTabPage(IParkitect parkitect, IAssetUpdatesManager assetUpdatesManager) : base(parkitect, AssetType.Mod)
        {
            _parkitect = parkitect;
            _assetUpdatesManager = assetUpdatesManager;

            assetUpdatesManager.UpdateFound += AssetUpdatesManager_UpdateFound;
            Text = "Mods";
        }

        private void AssetUpdatesManager_UpdateFound(object sender, AssetEventArgs e)
        {
            DrawUpdateAvailable(Controls.OfType<MetroTile>().FirstOrDefault(t => t.Tag.Equals(e.Asset)));
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

        #region Overrides of AssetsTabPage

        protected override void AddClickHandlerToTile(MetroTile tile, IAsset asset)
        {
            tile.Click +=
                (sender, args) =>
                {
                    var panel = ObjectFactory.With(asset as IModAsset).GetInstance<ModSliderPanel>();
                    (FindForm() as MainForm)?.SpawnSliderPanel(panel);
                };
        }

        #endregion
    }
}
