using System;
using MonoMac.AppKit;
using System.Drawing;
using MonoMac.CoreGraphics;

namespace ParkitectNexus.MacOSX
{
    public class MainMenuView : BaseView
    {
        private NSTextField _label1;

        public MainMenuView()
        {
            _label1 = new NSTextField(new Rectangle(83, 231, 300, 20)) {
                BackgroundColor = NSColor.Clear,
                TextColor = NSColor.Black,
                Editable = false,
                Bezeled = false,
                AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
                StringValue = "Disable, update or uninstall your mods.",
                Font = NSFont.SystemFontOfSize(10)
            };

            AddSubview(_label1);
        }


        public override void DrawRect(RectangleF dirtyRect)
        {
            var context = NSGraphicsContext.CurrentContext.GraphicsPort;
            context.SetStrokeColor (new CGColor(1.0f, 0, 0)); // red
            context.SetLineWidth (1.0F);
            context.StrokeEllipseInRect (dirtyRect);
        }

    }
}

