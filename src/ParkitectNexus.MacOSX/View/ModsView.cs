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
    public class ModsView : AssetsTilesView
    {
        public ModsView(IParkitect parkitect) : base(parkitect, AssetType.Mod)
        {
        }

        protected override void ClickedAsset(IAsset asset)
        {
            var mod = asset as IModAsset;
            this.Window.SetView<ModView,IModAsset>(mod);
        }
    }
    
}
