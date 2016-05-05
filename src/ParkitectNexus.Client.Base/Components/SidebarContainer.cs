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

using Xwt;
using Xwt.Drawing;

namespace ParkitectNexus.Client.Base.Components
{
    public class SidebarContainer : VBox
    {
        private const int WideWidth = 280;
        private readonly Button _closeButton;
        private readonly HBox _closeButtonBox;

        public SidebarContainer()
        {
            _closeButtonBox = new HBox();
            _closeButton = new Button("CLOSE")
            {
                Style = ButtonStyle.Flat,
                Image = App.Images["appbar.chevron.left.png"].WithSize(32),
                Font = Font.SystemFont.WithSize(20).WithStretch(FontStretch.UltraCondensed),
                ImagePosition = ContentPosition.Left
            };
            _closeButton.Clicked += (sender, args) => Clear();
            _closeButtonBox.PackStart(_closeButton);
        }

        public void ShowWidget(string name, Widget widget)
        {
            Clear();

            _closeButton.Label = name;

            if (widget != null)
            {
                PackStart(_closeButtonBox);
                PackStart(widget);
            }
        }
    }
}