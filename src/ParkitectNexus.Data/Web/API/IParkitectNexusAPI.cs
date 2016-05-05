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

using System.Threading.Tasks;

namespace ParkitectNexus.Data.Web.API
{
    /// <summary>
    ///     Contains the functionality of the ParkitectNexus API.
    /// </summary>
    public interface IParkitectNexusAPI
    {
        /// <summary>
        ///     Gets the asset with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The asset.</returns>
        Task<ApiAsset> GetAsset(string id);

        /// <summary>
        ///     Gets the subscriptions of the authenticated user.
        /// </summary>
        /// <param name="authKey">The authentication key.</param>
        /// <returns>The subscriptions.</returns>
        Task<ApiSubscription[]> GetSubscriptions(string authKey);

        /// <summary>
        ///     Gets user info of the authenticated user.
        /// </summary>
        /// <param name="authKey">The authentication key.</param>
        /// <returns>The user information.</returns>
        Task<ApiUser> GetUserInfo(string authKey);
    }
}
