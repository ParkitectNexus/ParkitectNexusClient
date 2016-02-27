// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Threading.Tasks;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web.API;

namespace ParkitectNexus.Data.Assets
{
    /// <summary>
    ///     Provides the functionality of an online Parkitect asset storage.
    /// </summary>
    public interface IRemoteAssetRepository
    {
        /// <summary>
        ///     Downloads the asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception">the specified asset id is invalid</exception>
        Task<IDownloadedAsset> DownloadAsset(ApiAsset asset);

        /// <summary>
        ///     Gets the latest mod tag.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <returns>The latest tag.</returns>
        Task<string> GetLatestModTag(IModAsset asset);
    }
}
