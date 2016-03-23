using System;
using MonoMac.AppKit;
using MonoMac.Foundation;
using System.Drawing;
using MonoMac.CoreGraphics;
using ParkitectNexus.Data.Assets.Modding;
using System.Threading.Tasks;
using System.Collections.Generic;
using ParkitectNexus.Data.Assets;
using System.Threading;
using ParkitectNexus.Data.Game;
using System.Linq;
using ParkitectNexus.Data.Utilities;
using System.IO;

namespace ParkitectNexus.MacOSX
{

    public class TileButtonCell : NSButtonCell
    {
        public TileButtonCell()
        {
            BackgroundColor = NSColor.FromDeviceRgba(0, 174 / 255f, 219 / 255f, 1);
            Bordered = false;
        }

        public override RectangleF TitleRectForBounds(RectangleF theRect)
        {
            RectangleF titleFrame = base.TitleRectForBounds(theRect);
            SizeF titleSize = AttributedStringValue.Size;

            return new RectangleF(titleFrame.X, theRect.Y + (theRect.Height - titleSize.Height) * 0.5f, titleFrame.Width, titleFrame.Height);
        }
    }
}
