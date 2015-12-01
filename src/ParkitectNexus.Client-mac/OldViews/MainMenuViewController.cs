
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using ParkitectNexus.Data;
using ParkitectNexus.Data.MacOSX;
using System.IO;
using ParkitectNexus.Client.Window;

namespace ParkitectNexus.Clientmac
{
    public partial class MainMenuViewController : MonoMac.AppKit.NSViewController
    {
        private MainWindow _mainWindow;
        private IParkitectNexusWebsite _parkitectNexusWebsite;
        private IParkitect _parkitect;

        #region Constructors

        // Called when created from unmanaged code
        public MainMenuViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }
		
        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public MainMenuViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }
		
        // Call to load from the XIB/NIB file
        public MainMenuViewController() : base("MainMenuView", NSBundle.MainBundle)
        {
            Initialize();
        }

        public MainMenuViewController(MainWindow mainWindow) : this()
        {
            if(mainWindow == null)
                throw new ArgumentNullException(nameof(mainWindow));
            _mainWindow = mainWindow;
        }
		
        // Shared initialization code
        void Initialize()
        {
            _parkitectNexusWebsite = new ParkitectNexusWebsite();
            _parkitect = new MacOSXParkitect();
        }

        #endregion

        //strongly typed view accessor
        public new MainMenuView View
        {
            get
            {
                return (MainMenuView)base.View;
            }
        }

        public override void LoadView()
        {
            base.LoadView();
        }

        partial void ClickedManageModsButton(MonoMac.Foundation.NSObject sender)
        {
            //_mainWindow.SetView(new ManageModsViewController(_mainWindow).View);
        }

        partial void ClickedLaunchParkitectButton(MonoMac.Foundation.NSObject sender)
        {
            if(!_parkitect.IsInstalled && !_parkitect.DetectInstallationPath())
            {
                var alert = new NSAlert() {
                    AlertStyle = NSAlertStyle.Warning,
                    InformativeText = "Not Installed :O",
                    MessageText = "Such wow messasche"
                };
                alert.BeginSheet(View.Window);
            }
            else
            {
                _parkitect.Launch();
            }
        }

        partial void ClickedVisitParkitectNexusButton(MonoMac.Foundation.NSObject sender)
        {
            _parkitectNexusWebsite.Launch();
            NSApplication.SharedApplication.Terminate(this);
        }

        partial void ClickedCloseButton(MonoMac.Foundation.NSObject sender)
        {
            NSApplication.SharedApplication.Terminate(this);
        }
    }
}

