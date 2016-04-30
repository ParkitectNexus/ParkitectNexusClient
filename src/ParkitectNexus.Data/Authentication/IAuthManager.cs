// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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