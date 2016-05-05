// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ParkitectNexus.Client.Base.Tiles;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using Xwt;
using Xwt.Drawing;
using Image = System.Drawing.Image;
using OperatingSystem = ParkitectNexus.Data.Utilities.OperatingSystem;

namespace ParkitectNexus.Client.Base.Pages
{
    public class MenuPageView : LoadableDataTileView
    {
        private readonly IParkitect _parkitect;
        private readonly IWebsite _website;

        public MenuPageView(IParkitect parkitect, ILogger log, IWebsite website, App app) : base(log, "Menu")
        {
            _parkitect = parkitect;
            _website = website;
        }

        private IEnumerable<Tile> LoadTiles()
        {
            yield return CreateTile("Visit ParkitectNexus", App.DImages["parkitectnexus_logo-64x64.png"],
                Color.FromBytes(0xf3, 0x77, 0x35), _website.Launch);

            if (OperatingSystem.Detect() == SupportedOperatingSystem.Linux)
                yield return
                    CreateTile("Download URL", App.DImages["appbar.browser.wire.png"], Color.FromBytes(0xf3, 0x77, 0x35),
                        () =>
                        {
                            var entry = new TextEntry();

                            var box = new HBox();
                            box.PackStart(new Label("URL:"));
                            box.PackStart(entry, true, true);

                            var dialog = new Dialog
                            {
                                Width = 300,
                                Icon = ParentWindow.Icon,
                                Title = "Enter URL to download",
                                Content = box
                            };
                            dialog.Buttons.Add(new DialogButton(Command.Cancel));
                            dialog.Buttons.Add(new DialogButton(Command.Ok));


                            var result = dialog.Run(ParentWindow);

                            NexusUrl url;
                            if (result.Label.ToLower() == "ok" && NexusUrl.TryParse(entry.Text, out url))
                                ObjectFactory.GetInstance<IApp>().HandleUrl(url);
                            dialog.Dispose();
                        });

            yield return
                CreateTile("Launch Parkitect", App.DImages["parkitect_logo.png"], Color.FromBytes(45, 137, 239),
                    () => { _parkitect.Launch(); });

            yield return CreateTile("Help", App.DImages["appbar.information.png"], Color.FromBytes(45, 137, 239), () =>
            {
                // Temporary help solution.
                Process.Start(
                    "https://parkitectnexus.com/forum/2/parkitectnexus-website-client/70/troubleshooting-mods-and-client");
            });

            yield return CreateTile("Donate!", App.DImages["appbar.thumbs.up.png"], Color.FromBytes(45, 137, 239), () =>
            {
                if (MessageDialog.AskQuestion("Maintaining this client and adding new features takes a lot of time.\n" +
                                              "If you appreciate our work, please consider sending a donation our way!\n" +
                                              "All donations will be used for further development of the ParkitectNexus Client and the website.\n" +
                                              "\nSelect Yes to visit PayPal and send a donation.", 1, Command.No,
                    Command.Yes) == Command.Yes)
                {
                    Process.Start("https://paypal.me/ikkentim");
                }
            });

        }

        #region Overrides of LoadableDataTileView

        protected override Task<IEnumerable<Tile>> LoadTiles(CancellationToken cancellationToken)
        {
            return Task.FromResult(LoadTiles());
        }

        #endregion

        public Tile CreateTile(string text, Image image, Color backgroundColor, Action clickedAction)
        {
            return new Tile(image, text, clickedAction) {BackgroundColor = backgroundColor};
        }
    }
}