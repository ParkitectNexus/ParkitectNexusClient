using System;
using MonoMac.AppKit;
using MonoMac.Foundation;
using System.Drawing;
using MonoMac.CoreGraphics;
using ParkitectNexus.Data.Assets.Modding;
using System.Threading.Tasks;
using System.Collections.Generic;
using ParkitectNexus.Data.Assets;
using System.Threading;
using ParkitectNexus.Data.Game;
using System.Linq;
using ParkitectNexus.Data.Utilities;
using System.IO;

namespace ParkitectNexus.MacOSX
{

    public class TilePresentedObject : NSObject
    {
        public string Name { get; set; }
        public Image Image { get; set; }

        public event EventHandler Click;

        public void HandleClick()
        {
            OnClick();
        }

        protected virtual void OnClick()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }
    }
    
}
