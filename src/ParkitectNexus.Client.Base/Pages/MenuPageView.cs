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
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;
using Xwt;
using Xwt.Drawing;
using Image = System.Drawing.Image;
namespace ParkitectNexus.Client.Base.Pages
{
    public class MenuPageView : LoadableDataTileView
    {
        private readonly IParkitect _parkitect;
        private readonly IWebsite _website;

        public MenuPageView(IParkitect parkitect, IWebsite website) : base("Menu")
        {
            _parkitect = parkitect;
            _website = website;
        }

        private IEnumerable<Tile> LoadTiles()
        {
            yield return CreateTile("Visit ParkitectNexus", App.DImages["parkitectnexus_logo-64x64.png"],
                Color.FromBytes(0xf3, 0x77, 0x35), _website.Launch);
            yield return CreateTile("Launch Parkitect", App.DImages["parkitect_logo.png"], Color.FromBytes(45, 137, 239),
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

//    public class MenuPageView : VBox, IPresenter, IPageView
//    {
//        private readonly HBox _line;
//
//        public MenuPageView(IParkitect parkitect, IWebsite website, IPresenter parent)
//        {
//            _line = new HBox {Margin = new WidgetSpacing(5, 5, 5, 5)};
//
//            AddButton(null, "Visit ParkitectNexus", App.Images["parkitectnexus_logo-64x64.png"],
//                Color.FromBytes(0xf3, 0x77, 0x35), website.Launch);
//            AddButton(null, "Launch Parkitect", App.Images["parkitect_logo.png"], Color.FromBytes(45, 137, 239),
//                () => { parkitect.Launch(); });
//            AddButton(null, "Help", App.Images["appbar.information.png"], Color.FromBytes(45, 137, 239), () =>
//            {
//                // Temporary help solution.
//                Process.Start(
//                    "https://parkitectnexus.com/forum/2/parkitectnexus-website-client/70/troubleshooting-mods-and-client");
//            });
//
//            AddButton(null, "Donate!", App.Images["appbar.thumbs.up.png"], Color.FromBytes(45, 137, 239), () =>
//            {
//                if (MessageDialog.AskQuestion("Maintaining this client and adding new features takes a lot of time.\n" +
//                                              "If you appreciate our work, please consider sending a donation our way!\n" +
//                                              "All donations will be used for further development of the ParkitectNexus Client and the website.\n" +
//                                              "\nSelect Yes to visit PayPal and send a donation.", 1, Command.No,
//                    Command.Yes) == Command.Yes)
//                {
//                    Process.Start("https://paypal.me/ikkentim");
//                }
//            });
//
//            PackStart(_line);
//        }
//
//        public void AddButton(string text, string tooltip, Image image, Color backgroundColor, Action clickedAction)
//        {
//            if (clickedAction == null) throw new ArgumentNullException(nameof(clickedAction));
//            var b = new Button
//            {
//                Image = image?.WithSize(64),
//                Label = text,
//                WidthRequest = 100,
//                HeightRequest = 100,
//                Font = Font.SystemFont.WithSize(12.4),
//                ImagePosition = ContentPosition.Top,
//                Style = ButtonStyle.Borderless,
//                BackgroundColor = backgroundColor,
//                TooltipText = tooltip
//            };
//
//            b.Clicked += (sender, args) => clickedAction();
//
//            _line.PackStart(b);
//        }
//
//        #region Implementation of IPageView
//
//        public string DisplayName { get; } = "Menu";
//
//        public event EventHandler DisplayNameChanged;
//
//        #endregion
//    }
}