// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.IO;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Game
{
    public class ParkitectAssetDataCacheFactory : IParkitectAssetDataCacheFactory
    {
        private IParkitectAssetDataCache<AssetCacheData> _blueprintCache;
        private IParkitectAssetDataCache<AssetCacheData> _savegameCache;

        #region Implementation of IParkitectAssetDataCacheFactory

        public IParkitectAssetDataCache<AssetCacheData> GetBlueprintCache()
        {
            return _blueprintCache ??
                   (_blueprintCache =
                       new ParkitectAssetDataCache<AssetCacheData>(Path.Combine(AppData.Path, "blueprintCache.json")));
        }

        public IParkitectAssetDataCache<AssetCacheData> GetSavegameCache()
        {
            return _savegameCache ??
                   (_savegameCache =
                       new ParkitectAssetDataCache<AssetCacheData>(Path.Combine(AppData.Path, "savegameCache.json")));
        }

        #endregion
    }
}
