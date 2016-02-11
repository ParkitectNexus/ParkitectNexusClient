// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

namespace ParkitectNexus.Data.Assets.Meta
{
    public interface IAssetMetadataStorage
    {
        IAssetMetadata GetMetadata(AssetType type, string path);
        void StoreMetadata(AssetType type, string path, IAssetMetadata metadata);
    }
}
