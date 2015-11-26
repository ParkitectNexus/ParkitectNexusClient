
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace ParkitectNexus.Clientmac
{
    public partial class ManageModsViewController : MonoMac.AppKit.NSWindowController
    {
        private MainWindow _mainWindow;

        #region Constructors

        // Called when created from unmanaged code
        public ManageModsViewController (IntPtr handle) : base (handle)
        {
            Initialize ();
        }
		
        // Called when created directly from a XIB file
        [Export ("initWithCoder:")]
        public ManageModsViewController (NSCoder coder) : base (coder)
        {
            Initialize ();
        }
		
        // Call to load from the XIB/NIB file
        public ManageModsViewController () : base ("ManageModsView")
        {
            Initialize ();
        }

        public ManageModsViewController(MainWindow mainWindow) : this()
        {
            if (mainWindow == null)
                throw new ArgumentNullException (nameof(mainWindow));
            _mainWindow = mainWindow;
        }

        // Shared initialization code
        void Initialize ()
        {
        }

        #endregion

        //strongly typed window accessor
        public new ManageModsView Window {
            get {
                return (ManageModsView)base.Window;
            }
        }
    }
}

