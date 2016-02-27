// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Assets
{
    public interface IAssetUpdatesManager
    {
        bool HasChecked { get; }

        event EventHandler<AssetEventArgs> UpdateFound;

        Task<int> CheckForUpdates();
        Task<bool> IsUpdateAvailableOnline(IAsset asset);
        bool IsUpdateAvailableInMemory(IAsset asset);
        Task<string> GetLatestVersionName(IAsset asset);
        bool ShouldCheckForUpdates();
    }
}
