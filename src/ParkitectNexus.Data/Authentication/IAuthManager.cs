// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Drawing;
using System.Threading.Tasks;
using ParkitectNexus.Data.Web.API;

namespace ParkitectNexus.Data.Authentication
{
    public interface IAuthManager
    {
        bool IsAuthenticated { get; }

        string Key { get; set; }

        event EventHandler Authenticated;

        Task<ApiUser> GetUser();
        Task<ApiSubscription[]> GetSubscriptions();
        Task<ApiAsset[]> GetSubscribedAssets();
        void OpenLoginPage();
        void ReloadKey();
        void Logout();
        Task<Image> GetAvatar();
    }
}
