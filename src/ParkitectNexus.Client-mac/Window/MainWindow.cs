
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using System.Drawing;
using ParkitectNexus.Client.View;

namespace ParkitectNexus.Client.Window
{
    [Register("MainWindow")]
    public class MainWindow : NSWindow
    {
        private NSImageView _banner;

        public MainWindow(RectangleF contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation)
            : base(contentRect, aStyle, bufferingType, deferCreation)
        {
            Title = "ParkitectNexus Client";

            ContentView = new NSView(Frame);

            _banner = new NSImageView(new Rectangle(0, 369 - 58 + 1, 493, 58)) {
                ImageScaling = NSImageScale.ProportionallyUpOrDown
            };

            using(var s = System.IO.File.OpenRead(NSBundle.MainBundle.PathForResource("dialog_banner", "png")))
                _banner.Image = NSImage.FromStream(s);
            
            ContentView.AddSubview(_banner);

            var v = new MainMenuView();
            ContentView.AddSubview(v);      
        }

        public void Terminate()
        {
            NSApplication.SharedApplication.Terminate(this);
        }
    }
}