
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using System.Drawing;
using ParkitectNexus.Client.View;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Game.MacOSX;

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
            var _parkitect = new MacOSXParkitect();
            _parkitect.DetectInstallationPath();

            if(url == null)
                throw new ArgumentNullException("url");

            if(DoNotDisturb)
                return;

            SetView(new InstallAssetView(url, _currentView));
        }

        public void Terminate()
        {
            NSApplication.SharedApplication.Terminate(this);
        }
    }
}