// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

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

        public Image Image { get; }

        public string Text { get; }

        public Action ClickAction { get; }
    }
}