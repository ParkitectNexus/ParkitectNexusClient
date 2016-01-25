// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using ParkitectNexus.Data.Caching;
using ParkitectNexus.Data.Settings;
using ParkitectNexus.Data.Settings.Models;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.API;

namespace ParkitectNexus.Data.Authentication
{
    public class ParkitectNexusAuthManager : IParkitectNexusAuthManager
    {
        private readonly ISettingsRepository<AuthSettings> _authSettingsRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IParkitectNexusWebsite _website;

        public ParkitectNexusAuthManager(IParkitectNexusWebsite website,
            ISettingsRepository<AuthSettings> authSettingsRepository, ICacheManager cacheManager)
        {
            if (website == null) throw new ArgumentNullException(nameof(website));
            if (authSettingsRepository == null) throw new ArgumentNullException(nameof(authSettingsRepository));
            if (cacheManager == null) throw new ArgumentNullException(nameof(cacheManager));
            _website = website;
            _authSettingsRepository = authSettingsRepository;
            _cacheManager = cacheManager;
        }

        public bool IsAuthenticated => !string.IsNullOrWhiteSpace(_authSettingsRepository.Model.APIKey);

        public string Key
        {
            get { return _authSettingsRepository.Model.APIKey; }
            set
            {
                _authSettingsRepository.Model.APIKey = value;
                _authSettingsRepository.Save();
            }
        }

        public async Task<ApiUser> GetUser()
        {
            AssertAuthenticated();

            var cached = _cacheManager.GetItem<ApiUser>("user");

            if (cached == null)
            {
                var real = await _website.API.GetUserInfo(Key);
                _cacheManager.SetItem("user", real);
                return real;
            }
            return cached;
        }

        public async Task<Image> GetAvatar()
        {
            var cached = _cacheManager.GetItem<AvatarCache>("avatar");

            if (cached == null || !cached.File.Exists)
            {
                using (var fullsize = await (await GetUser()).GetAvatar())
                {
                    if (fullsize == null)
                    {
                        _cacheManager.SetItem("avatar", new AvatarCache {HasAvatar = false});
                        return null;
                    }

                    var resized = ImageUtility.ResizeImage(fullsize, 32, 32);
                    var newCached = new AvatarCache {HasAvatar = true};

                    using (var fileStream = newCached.File.Open(FileMode.OpenOrCreate))
                    {
                        fileStream.SetLength(0);
                        resized.Save(fileStream, ImageFormat.Bmp);
                    }

                    _cacheManager.SetItem("avatar", newCached);
                    return resized;
                }
            }

            return cached.HasAvatar ? cached.GetImage() : null;
        }

        public async Task<ApiSubscription[]> GetSubscriptions()
        {
            AssertAuthenticated();
            return await _website.API.GetSubscriptions(_authSettingsRepository.Model.APIKey);
        }

        public async Task<ApiAsset[]> GetSubscribedAssets()
        {
            var assets = new List<ApiAsset>();
            var subscriptions = await GetSubscriptions();

            foreach (var subscription in subscriptions)
            {
                var asset = await subscription.GetAsset();
                if (asset != null)
                    assets.Add(asset);
            }

            return assets.ToArray();
        }

        public void OpenLoginPage()
        {
            _website.Launch("api/auth/login");
        }

        public void ReloadKey()
        {
            _authSettingsRepository.Load();
        }

        public void Logout()
        {
            Key = null;
            _cacheManager.SetItem<AvatarCache>("user", null);
            _cacheManager.SetItem<AvatarCache>("avatar", null);
        }

        private void AssertAuthenticated()
        {
            if (!IsAuthenticated)
                throw new Exception("No user authentication key has been set");
        }
    }
}
