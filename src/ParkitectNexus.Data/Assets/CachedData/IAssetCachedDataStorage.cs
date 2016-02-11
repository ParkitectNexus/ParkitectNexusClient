// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Threading.Tasks;
using ParkitectNexus.Data.Assets.Meta;

namespace ParkitectNexus.Data.Assets.CachedData
{
    public interface IAssetCachedDataStorage
    {
        Task<IAssetCachedData> GetData(AssetType type, IAssetMetadata metadata, string path);
        void StoreData(AssetType type, string path, IAssetCachedData metadata);
    }
}
