using System;
using MonoMac.AppKit;
using System.Drawing;
using ParkitectNexus.Data.Presenter;

namespace ParkitectNexus.MacOSX
{
    public class BaseView : NSView, IPresenter
    {
        public BaseView() : base(new Rectangle(0, 0, 600, 600))
        {
        }

        public override bool IsFlipped
        {
            get
            {
                return true;
            }
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

