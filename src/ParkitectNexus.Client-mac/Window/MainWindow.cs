
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using System.Drawing;
using ParkitectNexus.Client.View;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Game.MacOSX;
using ParkitectNexus.Client.Settings;

namespace ParkitectNexus.Client.Window
{
    [Register("MainWindow")]
    public class MainWindow : NSWindow
    {
        private NSImageView _banner;
        private BaseView _currentView;

        public MainWindow()
            : base(new Rectangle(0, 0, 493, 369), (NSWindowStyle.Titled | NSWindowStyle.Closable | NSWindowStyle.Miniaturizable), NSBackingStore.Buffered, false)
        {
            Title = "ParkitectNexus Client";

            ContentView = new NSView(Frame);

            _banner = new NSImageView(new Rectangle(0, 369 - 58 + 1, 493, 58)) {
                ImageScaling = NSImageScale.ProportionallyUpOrDown
            };

            using(var s = System.IO.File.OpenRead(NSBundle.MainBundle.PathForResource("dialog_banner", "png")))
                _banner.Image = NSImage.FromStream(s);
            
            ContentView.AddSubview(_banner);

            SetView(new MainMenuView());    

            Center();
        }

        public bool DoNotDisturb { get; set; }
        public bool IsClosing { get; private set; }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            if(CheckForUpdates(new ParkitectNexusWebsite()))
            {
                Terminate();
                return;
            }
        }


        private bool CheckForUpdates(IParkitectNexusWebsite website)
        {
            #if DEBUG
            return false;
            #else
            //var settings = new ClientSettings();
            var updateInfo = UpdateUtil.CheckForUpdates(website);

            if (updateInfo != null)
            {
                // TODO: Store download url so it can be downloaded after the update.

                var alert = new NSAlert()
                {
                    InformativeText = 
                        "A required update for the ParkitectNexus Client needs to be installed.\n" +
                        "Without this update you won't be able to install blueprints, savegames or mods trough this application. The update should take less than a minute.\n" +
                        $"Would you like to install it now?\n\nYou are currently running v{typeof (MainClass).Assembly.GetName().Version}. The newest version is v{updateInfo.Version}",
                    MessageText = "New version available!",
                    AlertStyle = NSAlertStyle.Informational,
                };

                alert.AddButton ("Ok");
                alert.AddButton ("Cancel");

                if(alert.RunSheetModal(this) != 1000)
                    return true;


                if (!UpdateUtil.InstallUpdate(updateInfo))
                {
                    alert = new NSAlert()
                    {
                        InformativeText = "Failed installing the update! Please try again later.",
                        MessageText = "Failure",
                        AlertStyle = NSAlertStyle.Critical
                    };

                    alert.BeginSheet(this);
                }
                return true;
            }

            return false;
            #endif
        }

        public void SetView(BaseView view)
        {
            if(view == null)
                throw new ArgumentNullException("view");
            
            if(_currentView == null)
            {
                ContentView.AddSubview(view);
            }
            else
            {
                ContentView.ReplaceSubviewWith(_currentView, view);
            }

            _currentView = view;
        }

        public void HandleInstall(ParkitectNexusUrl url)
        {
            var parkitect = new MacOSXParkitect();
            parkitect.DetectInstallationPath();

            if(url == null)
                throw new ArgumentNullException("url");

            if(DoNotDisturb || IsClosing)
                return;

            SetView(new InstallAssetView(url, _currentView));
        }

        public void Terminate()
        {
            IsClosing = true;
            NSApplication.SharedApplication.Terminate(this);
        }
    }
}