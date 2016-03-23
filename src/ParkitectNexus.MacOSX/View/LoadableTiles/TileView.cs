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

    public class TileView : NSView
    {
        private NSButton button;

        public TileView() : base(new RectangleF(0,0,110,110))
        {
            button = new NSButton(new RectangleF(5,5,100,100));
            button.SetButtonType(NSButtonType.MomentaryPushIn);
            button.Cell = new TileButtonCell();
         
            AddSubview(button);
        }

        public NSButton Button
        {
            get { return button; }  
        }       
    }

}
