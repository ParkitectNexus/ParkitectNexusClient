using System;
using MonoMac.AppKit;
using System.Drawing;

namespace ParkitectNexus.MacOSX
{
    public class BaseView : NSView
    {
        public BaseView() : base(new Rectangle(0, 0, 493, 311/*369 - 58*/))
        {
        }

        public new MainWindow Window
        {
            get
            {
                return (MainWindow)base.Window;
            }
        }

    }
}

