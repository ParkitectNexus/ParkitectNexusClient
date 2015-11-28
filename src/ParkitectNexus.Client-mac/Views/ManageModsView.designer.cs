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
	[Register ("ManageModsViewController")]
	partial class ManageModsViewController
	{
		[Outlet]
		MonoMac.AppKit.NSButton CheckForUpdatesButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton EnableModButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField ModLabel { get; set; }

		[Outlet]
		MonoMac.AppKit.NSScrollView ModsListScrollView { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton UninstallButton { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField VersionLabel { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton ViewOnParkitectNexusButton { get; set; }

		[Action ("ClickedBackButton:")]
		partial void ClickedBackButton (MonoMac.Foundation.NSObject sender);

		[Action ("ClickedCheckForUpdatesButton:")]
		partial void ClickedCheckForUpdatesButton (MonoMac.Foundation.NSObject sender);

		[Action ("ClickedEnableModButton:")]
		partial void ClickedEnableModButton (MonoMac.Foundation.NSObject sender);

		[Action ("ClickedUninstallButton:")]
		partial void ClickedUninstallButton (MonoMac.Foundation.NSObject sender);

		[Action ("ClickedViewOnParkitectNexusButton:")]
		partial void ClickedViewOnParkitectNexusButton (MonoMac.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ModsListScrollView != null) {
				ModsListScrollView.Dispose ();
				ModsListScrollView = null;
			}

			if (ModLabel != null) {
				ModLabel.Dispose ();
				ModLabel = null;
			}

			if (VersionLabel != null) {
				VersionLabel.Dispose ();
				VersionLabel = null;
			}

			if (ViewOnParkitectNexusButton != null) {
				ViewOnParkitectNexusButton.Dispose ();
				ViewOnParkitectNexusButton = null;
			}

			if (EnableModButton != null) {
				EnableModButton.Dispose ();
				EnableModButton = null;
			}

			if (UninstallButton != null) {
				UninstallButton.Dispose ();
				UninstallButton = null;
			}

			if (CheckForUpdatesButton != null) {
				CheckForUpdatesButton.Dispose ();
				CheckForUpdatesButton = null;
			}
		}
	}

	[Register ("ManageModsView")]
	partial class ManageModsView
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
