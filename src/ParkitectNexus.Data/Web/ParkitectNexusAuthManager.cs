// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Threading.Tasks;
using ParkitectNexus.Data.Settings;
using ParkitectNexus.Data.Settings.Models;
using ParkitectNexus.Data.Web.API;

namespace ParkitectNexus.Data.Web
{
    public interface IParkitectNexusAuthManager
    {
        bool IsAuthenticated { get; }
        string Key { get; set; }
        Task<ApiUser> GetUser();
        Task<ApiSubscription[]> GetSubscriptions();
        void OpenLoginPage();
        void ReloadKey();
        void Logout();
    }

    public class ParkitectNexusAuthManager : IParkitectNexusAuthManager
    {
        private readonly ISettingsRepository<AuthSettings> _authSettingsRepository;
        private readonly IParkitectNexusWebsite _website;

        public ParkitectNexusAuthManager(IParkitectNexusWebsite website,
            ISettingsRepository<AuthSettings> authSettingsRepository)
        {
            if (website == null) throw new ArgumentNullException(nameof(website));
            if (authSettingsRepository == null) throw new ArgumentNullException(nameof(authSettingsRepository));
            _website = website;
            _authSettingsRepository = authSettingsRepository;
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
        public Task<ApiUser> GetUser()
        {
            AssertAuthenticated();
            throw new NotImplementedException("not implemented in API yet");
        }

        public async Task<ApiSubscription[]> GetSubscriptions()
        {
            AssertAuthenticated();
            return await _website.API.GetSubscriptions(_authSettingsRepository.Model.APIKey);
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
        }

        private void AssertAuthenticated()
        {
            if (!IsAuthenticated)
                throw new Exception("No user authentication key has been set");
        }
    }
}
