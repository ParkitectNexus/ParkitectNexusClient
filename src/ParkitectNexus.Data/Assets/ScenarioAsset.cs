using System;
using System.Drawing;
using ParkitectNexus.Data.Assets.CachedData;
using ParkitectNexus.Data.Assets.Meta;

namespace ParkitectNexus.Data.Assets
{
    public class ScenarioAsset : Asset
    {
        private readonly AssetWithImageCachedData _cachedData;

        public ScenarioAsset(string path, IAssetMetadata metadata, AssetWithImageCachedData cachedData)
            : base(path, metadata, cachedData, AssetType.Scenario)
        {
            if (cachedData == null) throw new ArgumentNullException(nameof(cachedData));
            _cachedData = cachedData;
        }

        #region Overrides of Asset

        public override Image GetImage()
        {
            return _cachedData.GetImage();
        }

        #endregion
    }
}
