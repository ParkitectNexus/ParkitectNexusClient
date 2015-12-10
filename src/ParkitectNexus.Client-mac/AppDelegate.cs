using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using ParkitectNexus.Client.Window;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Client.View;

namespace ParkitectNexus.Clientmac
{
    public partial class AppDelegate : NSApplicationDelegate
    {
        private MainWindowController _mainWindowController;
        private ParkitectNexusUrl _url;

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
            Log.WriteLine($"Finished launching with notification {notification}");

            _mainWindowController = new MainWindowController();
            _mainWindowController.Window.MakeKeyAndOrderFront(this);
            if(_url != null)
                _mainWindowController.Window.SetView(new InstallAssetView(_url, null));
        }

        public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
        {
            return true;
        }

        [Export("handleGetURLEvent:withReplyEvent:")]
        private void HandleGetURLEvent(NSAppleEventDescriptor descriptor, NSAppleEventDescriptor replyEvent)
        {   
            Log.WriteLine($"HandleGetURLEvent: {descriptor}");
            for (int i = 1; i <= descriptor.NumberOfItems; i++) {
                var innerDesc = descriptor.DescriptorAtIndex (i);
                if(!string.IsNullOrEmpty(innerDesc.StringValue))
                {
                    ParkitectNexusUrl url;
                    if(ParkitectNexusUrl.TryParse(innerDesc.StringValue, out url))
                    {
                        if(_mainWindowController == null)
                            _url = url;
                        else
                        _mainWindowController.Window.HandleInstall(url);
                    }
                }
            }
        }
    }
}

