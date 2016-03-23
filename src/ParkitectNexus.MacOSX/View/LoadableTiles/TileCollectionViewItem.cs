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

    public class TileCollectionViewItem : NSCollectionViewItem
    {
        private TileView _view;
        public TileCollectionViewItem() : base()
        {
        }

        public TileCollectionViewItem(IntPtr ptr) : base(ptr)
        {

        }

        public override void LoadView ()
        {
            _view = new TileView();
            View = _view;
        }

        public override NSObject RepresentedObject 
        {
            get { return base.RepresentedObject; }

            set 
            {
                TilePresentedObject tile = value as TilePresentedObject;

                if (tile == null)
                {
                    base.RepresentedObject = new NSString(string.Empty);
                    _view.Button.Title = string.Empty;
                }
                else
                {
                    base.RepresentedObject = value;
                    _view.Button.Title = tile.Name;
                    _view.Button.Image = tile.Image == null ? null : tile.Image.ToNSImage();
                    _view.Button.Activated += (sender, e) =>
                    {
                        tile.HandleClick();
                    };
                }
            }
        }
    }
    
}
