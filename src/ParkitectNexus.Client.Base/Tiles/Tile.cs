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
using System.Drawing;

namespace ParkitectNexus.Client.Base.Tiles
{
    public class Tile
    {
        public Tile(Image image, string text, Action clickAction)
        {
            if (clickAction == null) throw new ArgumentNullException(nameof(clickAction));
            Image = image;
            Text = text;
            ClickAction = clickAction;
        }

        // TODO better use xwt image
        public Image Image { get; }

        public string Text { get; }

        public Action ClickAction { get; }

        public Xwt.Drawing.Color BackgroundColor { get; set; } = Xwt.Drawing.Color.FromBytes(45, 137, 239);
    }
}
