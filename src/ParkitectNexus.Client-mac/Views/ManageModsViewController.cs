
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using ParkitectNexus.Data;
using ParkitectNexus.Data.MacOSX;

namespace ParkitectNexus.Clientmac
{
    public partial class ManageModsViewController : MonoMac.AppKit.NSViewController
    {
        private MainWindow _mainWindow;
        private IParkitectNexusWebsite _parkitectNexusWebsite;
        private IParkitect _parkitect;

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
        public ManageModsViewController () : base ("ManageModsView", NSBundle.MainBundle)
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
            _parkitectNexusWebsite = new ParkitectNexusWebsite();
            _parkitect = new MacOSXParkitect();
        }

        #endregion

        //strongly typed view accessor
        public new ManageModsView View {
            get {
                return (ManageModsView)base.View;
            }
        }

        public override void AwakeFromNib ()
        {
            base.AwakeFromNib ();

            ModLabel.StringValue = "-";
            VersionLabel.StringValue = "-";
            ViewOnParkitectNexusButton.Enabled = false;
            EnableModButton.Enabled = false;
            CheckForUpdatesButton.Enabled = false;
            UninstallButton.Enabled = false;
            //ModsListScrollView.
        }

        partial void ClickedBackButton (MonoMac.Foundation.NSObject sender)
        {
            _mainWindow.SetView(new MainMenuViewController(_mainWindow).View);
        }
    }
}

