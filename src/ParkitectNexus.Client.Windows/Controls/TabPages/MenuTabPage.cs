// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Controls;
using ParkitectNexus.Client.Windows.Properties;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;

namespace ParkitectNexus.Client.Windows.Controls.TabPages
{
    public class MenuTabPage : LoadableTilesTabPage
    {
        private readonly IParkitect _parkitect;
        private readonly IWebsite _website;

        public MenuTabPage(IParkitect parkitect, IWebsite website)
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
            var visit = new MetroTile
            {
                Text = "Visit",
                TextAlign = ContentAlignment.BottomCenter,
                Style = MetroColorStyle.Orange,
                TileImage = Resources.parkitectnexus_logo,
                UseTileImage = true,
                TileImageAlign = ContentAlignment.MiddleCenter
            };

            var launch = new MetroTile
            {
                Text = "Launch",
                TextAlign = ContentAlignment.BottomCenter,
                Style = MetroColorStyle.Default,
                TileImage = Resources.parkitect_logo,
                UseTileImage = true,
                TileImageAlign = ContentAlignment.MiddleCenter
            };

            var help = new MetroTile
            {
                Text = "Help",
                TextAlign = ContentAlignment.BottomCenter,
                Style = MetroColorStyle.Default,
                TileImage = Resources.appbar_information,
                UseTileImage = true,
                TileImageAlign = ContentAlignment.MiddleCenter
            };

            var donate = new MetroTile
            {
                Text = "Donate",
                TextAlign = ContentAlignment.BottomCenter,
                Style = MetroColorStyle.Default,
                TileImage = Resources.appbar_thumbs_up,
                UseTileImage = true,
                TileImageAlign = ContentAlignment.MiddleCenter
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
                // Temporary help solution.
                Process.Start(
                    "https://parkitectnexus.com/forum/2/parkitectnexus-website-client/70/troubleshooting-mods-and-client");
            };

            donate.Click += (sender, args) =>
            {

                if (MetroMessageBox.Show(GetParentForm(), "Maintaining this client and adding new features takes a lot of time.\n" +
                                      "If you appreciate our work, please consider sending a donation our way!\n" +
                                      "All donations will be used for further development of the ParkitectNexus Client and the website.\n" +
                                      "\nSelect Yes to visit PayPal and send a donation.", "ParkitectNexus Client",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Process.Start("https://paypal.me/ikkentim");
                }
            };

            return Task.FromResult((IEnumerable<MetroTile>) new[]
            {
                visit, launch, help, donate
            });
        }

        #endregion

        private Form GetParentForm()
        {
            var form = Parent;

            while (!(form is Form) && form != null)
            {
                form = form.Parent;
            }

            return form as Form;
        }
    }

}
