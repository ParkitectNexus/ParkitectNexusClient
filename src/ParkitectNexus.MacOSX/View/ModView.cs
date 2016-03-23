using System;
using MonoMac.AppKit;
using System.Drawing;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Utilities;
using MonoMac.Foundation;

namespace ParkitectNexus.MacOSX
{
    public class ModView : BaseView
    {
        private IModAsset _mod;

        private NSTextField _label1;
        private NSTextField _label2;
        private NSImageView _imageView;

        public ModView(IModAsset mod)
        {
            _mod = mod;
            
            var image = mod.GetImage();
            if(image == null)
            {
                var path = NSBundle.MainBundle.PathForResource("no_image_available", "png");
                image = Image.FromFile(path);
            }

            _label1 = new NSTextField(new Rectangle(10, 10, 300, 20)) {
                BackgroundColor = NSColor.Clear,
                TextColor = NSColor.Black,
                Editable = false,
                Bezeled = false,
                AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
                StringValue = mod.Name,
                Font = NSFont.BoldSystemFontOfSize(16)
            };
            _imageView = new NSImageView(new Rectangle(10, 30, 350, 350)) {
                Image = new NSImage(NSBundle.MainBundle.PathForResource("no_image_available", "png")),//ImageUtility.ResizeImage(mod.GetImage(), 350, 350).ToNSImage(),
//                ImageAlignment = NSImageAlignment.Center,
                ImageFrameStyle = NSImageFrameStyle.None,
                Bounds = new Rectangle(10, 40, 300, 300)
            };
            _label2 = new NSTextField(new Rectangle(10, 380, 300, 20)) {
                BackgroundColor = NSColor.Clear,
                TextColor = NSColor.Black,
                Editable = false,
                Bezeled = false,
                AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
                StringValue = "Foo: Bar",
                Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize)
            };

            AddSubview(_label1);
            AddSubview(_imageView);
            AddSubview(_label2);
        }
    }
}

