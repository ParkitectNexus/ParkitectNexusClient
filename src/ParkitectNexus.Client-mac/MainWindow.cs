
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace ParkitectNexus.Clientmac
{
    public partial class MainWindow : MonoMac.AppKit.NSWindow
    {
        #region Constructors

        // Called when created from unmanaged code
        public MainWindow (IntPtr handle) : base (handle)
        {
            Initialize ();
        }

        // Called when created directly from a XIB file
        [Export ("initWithCoder:")]
        public MainWindow (NSCoder coder) : base (coder)
        {
            Initialize ();
        }

        // Shared initialization code
        void Initialize ()
        {
        }

        #endregion

        public void SetView(NSView view)
        {
            if (view == null)
                throw new ArgumentNullException (nameof(view));
            
            if (CustomView.Subviews.Any ()) {
                CustomView.ReplaceSubviewWith (CustomView.Subviews.First (), view);
            } else {
                CustomView.AddSubview (view);
            }
        }
        public override void AwakeFromNib()
        {
            base.AwakeFromNib ();

            using (var s = System.IO.File.OpenRead (NSBundle.MainBundle.PathForResource ("dialog_banner", "png")))
                BannerImageView.Image = NSImage.FromStream (s);
            
            SetView(new MainMenuViewController(this).View);
        }
    }
}

