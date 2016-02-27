using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Caching;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Data.Assets
{
    public interface IAssetUpdatesManager
    {
        event EventHandler<AssetEventArgs> UpdateFound;
        Task<int> CheckForUpdates();
        Task<bool> IsUpdateAvailableOnline(IAsset asset);
        bool IsUpdateAvailableInMemory(IAsset asset);
        Task<string> GetLatestVersionName(IAsset asset);
        bool ShouldCheckForUpdates();
        bool HasChecked { get; }
    }

    public class AssetUpdatesManager : IAssetUpdatesManager
    {
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1);

        private event EventHandler<AssetEventArgs> BaseUpdateFound;

        public bool HasChecked { get; private set; }

        private bool _isChecking = false;
        private IDictionary<IAsset,string> _updatesAvailable = new Dictionary<IAsset,string>();
        private readonly IRemoteAssetRepository _remoteAssetRepository;
        private readonly IParkitect _parkitect;
        private readonly ICacheManager _cacheManager;

        public AssetUpdatesManager(IRemoteAssetRepository remoteAssetRepository, IParkitect parkitect, ICacheManager cacheManager)
        {
            _remoteAssetRepository = remoteAssetRepository;
            _parkitect = parkitect;
            _cacheManager = cacheManager;
        }

        public event EventHandler<AssetEventArgs> UpdateFound
        {
            add
            {
                BaseUpdateFound += value;

                if (HasChecked)
                {
                    foreach(var asset in _updatesAvailable.Keys)
                        value(this, new AssetEventArgs(asset));
                }
            }
            remove { BaseUpdateFound -= value; }
        }

        public bool ShouldCheckForUpdates()
        {
            var cache = _cacheManager.GetItem<AssetUpdatesCache> ("updates_check");

            if (cache == null || DateTime.Now - cache.CheckedDate > _checkInterval)
                return true;

            if (_isChecking)
                return false;

            ReadFromCache();

            return !HasChecked;
        }

        private void ReadFromCache()
        {
            if (HasChecked)
                return;

            var cache = _cacheManager.GetItem<AssetUpdatesCache>("updates_check");

            if (cache == null)
                return;

            _updatesAvailable = cache.ToAssetsList(_parkitect);
            HasChecked = true;
        }

        public async Task<int> CheckForUpdates()
        {
            _isChecking = true;
            var count = 0;
            try
            {
                foreach (var asset in _parkitect.Assets[AssetType.Mod].OfType<ModAsset>())
                {
                    if (asset?.Repository == null || asset.Information.IsDevelopment)
                        continue;

                    var latestTag = await _remoteAssetRepository.GetLatestModTag(asset);

                    if (latestTag != null && asset.Tag != latestTag)
                    {
                        _updatesAvailable.Add(asset, latestTag);
                        count++;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            _cacheManager.SetItem("updates_check", AssetUpdatesCache.FromAssetsList(_updatesAvailable));

            HasChecked = true;

            return count;
        }

        public async Task<bool> IsUpdateAvailableOnline(IAsset asset)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));

            switch (asset.Type)
            {
                case AssetType.Blueprint:
                case AssetType.Savegame:
                    return false;
                case AssetType.Mod:
                    var modAsset = asset as IModAsset;
                    var latestTag = await _remoteAssetRepository.GetLatestModTag(modAsset);
                    return latestTag != null && latestTag != modAsset.Tag;
                default:
                    throw new ArgumentException("invalid asset type", nameof(asset));
            }
        }

        public bool IsUpdateAvailableInMemory(IAsset asset)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));

            ReadFromCache();

            switch (asset.Type)
            {
                case AssetType.Blueprint:
                case AssetType.Savegame:
                    return false;
                case AssetType.Mod:
                    return HasChecked && _updatesAvailable.ContainsKey(asset);

                default:
                    throw new ArgumentException("invalid asset type", nameof(asset));
            }
        }

        public async Task<string> GetLatestVersionName(IAsset asset)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));

            ReadFromCache();

            switch (asset.Type)
            {
                case AssetType.Blueprint:
                case AssetType.Savegame:
                    return null;
                case AssetType.Mod:
                    var modAsset = asset as IModAsset;

                    if (!HasChecked)
                        return await _remoteAssetRepository.GetLatestModTag(modAsset);

                    string tag;
                    return _updatesAvailable.TryGetValue(asset, out tag) ? tag : modAsset.Tag;
                default:
                    throw new ArgumentException("invalid asset type", nameof(asset));
            }
        }

        protected virtual void OnUpdateFound(AssetEventArgs e)
        {
            BaseUpdateFound?.Invoke(this, e);
        }

        private class AssetUpdatesCache
        {
            public IList<AssetCachedUpdateInfo> Updates { get; set; }

            public DateTime CheckedDate { get; set; }

            public IDictionary<IAsset, string> ToAssetsList(IParkitect parkitect)
            {
                if (Updates == null)
                    return null;

                var result = new Dictionary<IAsset, string>();
                foreach (var keyValue in Updates)
                {
                    var asset = parkitect.Assets[keyValue.Type].FirstOrDefault(a => a.Id == keyValue.Id);

                    if (asset != null)
                        result[asset] = keyValue.Tag;
                }

                return result;
            }

            public static AssetUpdatesCache FromAssetsList(IDictionary<IAsset, string> updatesAvailable)
            {
                if (updatesAvailable == null) throw new ArgumentNullException(nameof(updatesAvailable));
                var result = new AssetUpdatesCache
                {
                    CheckedDate = DateTime.Now,
                    Updates = new List<AssetCachedUpdateInfo>()
                };

                foreach (var keyValue in updatesAvailable)
                    result.Updates.Add(new AssetCachedUpdateInfo
                    {
                        Type = keyValue.Key.Type,
                        Id = keyValue.Key.Id,
                        Tag = keyValue.Value
                    });

                return result;
            }
        }

        private class AssetCachedUpdateInfo
        {
            public AssetType Type { get; set; }
            public string Id { get; set; }
            public string Tag { get; set; }
        }
    }
}
