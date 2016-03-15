using System;
using MonoMac.Foundation;

namespace ParkitectNexus.MacOSX
{
    [Register("MainWindowController")]
    public partial class MainWindowController : MonoMac.AppKit.NSWindowController
    {
        #region Constructors

        public MainWindowController(IntPtr handle) : base(handle)
        {
        }

        [Export("initWithCoder:")]
        public MainWindowController(NSCoder coder) : base(coder)
        {
        }

        public MainWindowController() : base("MainWindow")
        {
            base.Window = new MainWindow();
            Window.AwakeFromNib();
        }

        #endregion

        public new MainWindow Window
        {
            get
            {
                return (MainWindow)base.Window;
            }
        }
    }
}

