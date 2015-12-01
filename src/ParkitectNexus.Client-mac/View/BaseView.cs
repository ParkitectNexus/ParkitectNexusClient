using System;
using MonoMac.AppKit;
using System.Drawing;
using ParkitectNexus.Client.Window;

namespace ParkitectNexus.Client.View
{
    public class BaseView : NSView
    {
        public BaseView() : base(new Rectangle(0, 0, 493, 369 - 58))
        {
            var box = new NSBox(new Rectangle(0, 58, 488, 1));
            box.BoxType = NSBoxType.NSBoxSeparator;
            box.Title = string.Empty;

            AddSubview(box);

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
