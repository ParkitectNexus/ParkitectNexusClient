// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;
using System.CodeDom.Compiler;

namespace ParkitectNexus.Clientmac
{
	[Register ("MainWindow")]
	partial class MainWindow
	{
		[Outlet]
		MonoMac.AppKit.NSImageView BannerImageView { get; set; }

		[Outlet]
		MonoMac.AppKit.NSView CustomView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (BannerImageView != null) {
				BannerImageView.Dispose ();
				BannerImageView = null;
			}

			if (CustomView != null) {
				CustomView.Dispose ();
				CustomView = null;
			}
		}
	}

	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
