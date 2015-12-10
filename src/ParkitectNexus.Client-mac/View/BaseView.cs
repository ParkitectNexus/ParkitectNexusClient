using System;
using MonoMac.AppKit;
using System.Drawing;
using ParkitectNexus.Client.Window;
using System.Diagnostics;

namespace ParkitectNexus.Client.View
{
    public class BaseView : NSView
    {
        private NSButton _donateButton;

        public BaseView() : base(new Rectangle(0, 0, 493, 311/*369 - 58*/))
        {
            var box = new NSBox(new Rectangle(0, 58, 488, 1));
            box.BoxType = NSBoxType.NSBoxSeparator;
            box.Title = string.Empty;

            _donateButton = new NSButton(new Rectangle(6, 6, 81, 32)) {
                AutoresizingMask = NSViewResizingMask.MinYMargin,
                BezelStyle = NSBezelStyle.Rounded,
                Bordered = false,
                Title = "Donate",
            };

            AddSubview(_donateButton);
            AddSubview(box);
        }

        public new MainWindow Window
        {
            get
            {
                return (MainWindow)base.Window;
            }
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            _donateButton.Activated += (sender, e) =>
            {
                var alert = new NSAlert() {
                    AlertStyle = NSAlertStyle.Informational,
                    InformativeText = "Maintaining this client and adding new features takes a lot of time.\n" +
                        "If you appreciate our work, please consider sending a donation our way!\n" +
                        "All donations will be used for further development of the ParkitectNexus Client and the website.",
                    MessageText = "Donating",
                };
                alert.AddButton ("Ok");
                alert.AddButton ("Cancel");

                alert.BeginSheetForResponse (Window, (result) => {
                    if(result == 1000) {
                        Process.Start("https://paypal.me/ikkentim");
                    }
                });
            };
        }
    }
}
