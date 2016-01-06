using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Controls;
using ParkitectNexus.Client.Windows.Properties;
using ParkitectNexus.Client.Windows.SliderPanels;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;

namespace ParkitectNexus.Client.Windows.TabPages
{
    public class MenuTabPage : LoadableTilesTabPage
    {
        private readonly IParkitect _parkitect;
        private readonly IParkitectNexusWebsite _website;

        public MenuTabPage(IParkitect parkitect, IParkitectNexusWebsite website)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (website == null) throw new ArgumentNullException(nameof(website));

            _parkitect = parkitect;
            _website = website;

            Text = "Menu";

        }

        #region Overrides of LoadableTilesTabPage

        protected override bool ReloadOnEnter { get; } = false;

        protected override Task<IEnumerable<MetroTile>> LoadTiles(CancellationToken cancellationToken)
        {
            var visit = new MetroTile()
            {
                Text = "Visit",
                TextAlign = ContentAlignment.BottomCenter,
                Style = MetroColorStyle.Orange,
                TileImage = Resources.parkitectnexus_logo,
                UseTileImage = true,
                TileImageAlign = ContentAlignment.MiddleCenter,
            };

            var launch = new MetroTile()
            {
                Text = "Launch",
                TextAlign = ContentAlignment.BottomCenter,
                Style = MetroColorStyle.Default,
                TileImage = Resources.parkitect_logo,
                UseTileImage = true,
                TileImageAlign = ContentAlignment.MiddleCenter,
            };

            var help = new MetroTile()
            {
                Text = "Help",
                TextAlign = ContentAlignment.BottomCenter,
                Style = MetroColorStyle.Default,
                TileImage = Resources.appbar_information,
                UseTileImage = true,
                TileImageAlign = ContentAlignment.MiddleCenter,
            };

            visit.Click += (sender, args) =>
            {
                _website.Launch();
                Application.Exit();
            };
            launch.Click += (sender, args) =>
            {
                _parkitect.Launch();
                Application.Exit();
            };
            help.Click += (sender, args) =>
            {
            };

            return Task.FromResult((IEnumerable<MetroTile>)new[]
            {
                visit, launch, help
            });
        }

        #endregion
    }
}
