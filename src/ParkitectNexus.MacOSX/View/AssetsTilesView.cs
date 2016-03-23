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

    public abstract class AssetsTilesView : LoadableTilesView
    {
        IParkitect _parkitect;
        AssetType _type;

        public AssetsTilesView(IParkitect parkitect, AssetType type)
        {
            _type = type;
            _parkitect = parkitect;
        }

        protected abstract void ClickedAsset(IAsset asset);

        protected override Task<IEnumerable<TilePresentedObject>> LoadTiles(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var tiles = new List<TilePresentedObject>();

                var current = 0;
                var fileCount = _parkitect.Assets.GetAssetCount(_type);
                foreach (var asset in _parkitect.Assets[_type])
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var tile = new TilePresentedObject
                    {
                        Name = asset.Name,
                        Image = ImageUtility.ResizeImage(asset.GetImage(), 100, 100)
                    };

                    tile.Click += (sender, e) => {
                        ClickedAsset(asset);
                    };

                    tiles.Add(tile);

                    UpdateLoadingProgress((current++*100)/fileCount);
                }
                return (IEnumerable<TilePresentedObject>) tiles;
            }, cancellationToken);
        }

    }
    
}
