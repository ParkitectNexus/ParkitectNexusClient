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
            return Image.FromStream(Open());
        }

        #endregion
    }
}
