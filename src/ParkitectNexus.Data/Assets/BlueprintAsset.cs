// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Drawing;
using ParkitectNexus.Data.Assets.CachedData;
using ParkitectNexus.Data.Assets.Meta;

namespace ParkitectNexus.Data.Assets
{
    public class BlueprintAsset : Asset
    {
        public BlueprintAsset(string path, IAssetMetadata metadata, IAssetCachedData cachedData)
            : base(path, metadata, cachedData, AssetType.Blueprint)
        {
        }

        #region Overrides of Asset

        public override Image GetImage()
        {
            using (var stream = Open())
            {
                if (stream == null)
                    return null;

                using (var image = Image.FromStream(stream))
                {
                    var result = new Bitmap(image.Width, image.Height);
                    using (var graphics = Graphics.FromImage(result))
                        graphics.DrawImage(image, 0, 0);

                    return result;
                }
            }
        }

        #endregion
    }
}
