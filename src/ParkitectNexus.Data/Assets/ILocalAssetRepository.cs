// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Data.Assets
{
    public interface ILocalAssetRepository : IEnumerable<IAsset>
    {
        IEnumerable<Asset> this[AssetType type] { get; }

        event EventHandler<AssetEventArgs> AssetAdded;

        event EventHandler<AssetEventArgs> AssetRemoved;

        int GetAssetCount(AssetType type);
        Task<IAsset> StoreAsset(IDownloadedAsset downloadedAsset);
        void DeleteAsset(IAsset asset);
    }
}
