using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using System.Drawing;

namespace ParkitectNexus.Client.Window
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

