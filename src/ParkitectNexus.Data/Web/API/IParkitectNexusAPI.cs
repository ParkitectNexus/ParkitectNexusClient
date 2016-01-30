﻿// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

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
