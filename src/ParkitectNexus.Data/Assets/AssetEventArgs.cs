using System;

namespace ParkitectNexus.Data.Assets
{
    public class AssetEventArgs : EventArgs
    {
        public AssetEventArgs(IAsset asset)
        {
            Asset = asset;
        }

        public IAsset Asset { get; }
    }
}
