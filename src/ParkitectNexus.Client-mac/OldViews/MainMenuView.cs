
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace ParkitectNexus.Clientmac
{
    public partial class MainMenuView : MonoMac.AppKit.NSView
    {
        #region Constructors

        // Called when created from unmanaged code
        public MainMenuView(IntPtr handle) : base(handle)
        {
            Initialize();
        }
		
        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public MainMenuView(NSCoder coder) : base(coder)
        {
            Initialize();
        }
		
        // Shared initialization code
        void Initialize()
        {
            
        }

        #endregion
    }
}

