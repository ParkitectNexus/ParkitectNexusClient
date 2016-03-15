using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

namespace ParkitectNexus.MacOSX
{
    public partial class AppDelegate : NSApplicationDelegate
    {
        MainWindowController _mainWindowController;

        public AppDelegate()
        {
        }

        public override void WillFinishLaunching(NSNotification notification)
        {
            var appleEventManager = NSAppleEventManager.SharedAppleEventManager;
            appleEventManager.SetEventHandler(this, new Selector("handleGetURLEvent:withReplyEvent:"), AEEventClass.Internet, AEEventID.GetUrl);
        }

        public override void FinishedLaunching(NSObject notification)
        {
            _mainWindowController = new MainWindowController();
            _mainWindowController.Window.MakeKeyAndOrderFront(this);
        }

        public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
        {
            return true;
        }

        [Export("handleGetURLEvent:withReplyEvent:")]
        private void HandleGetURLEvent(NSAppleEventDescriptor descriptor, NSAppleEventDescriptor replyEvent)
        {   
        }
    }
}

