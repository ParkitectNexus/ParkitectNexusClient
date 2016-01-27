// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Data.Assets
{
    public interface ILocalAssetsRepository
    {
        event EventHandler<AssetEventArgs> AssetAdded;
        event EventHandler<AssetEventArgs> AssetRemoved;
        IEnumerable<Asset> GetAssets(AssetType type);
        int GetAssetCount(AssetType type);
        Task StoreAsset(IDownloadedAsset downloadedAsset);
    }
}
