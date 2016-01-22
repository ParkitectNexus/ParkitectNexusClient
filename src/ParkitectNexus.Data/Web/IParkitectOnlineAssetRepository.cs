// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Threading.Tasks;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Data.Web
{
    /// <summary>
    ///     Provides the functionality of an online Parkitect asset storage.
    /// </summary>
    public interface IParkitectOnlineAssetRepository
    {
        /// <summary>
        ///     Downloads the file associated with the specified url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>An instance which performs the requested task.</returns>
        Task<IParkitectDownloadedAsset> DownloadFile(IParkitectNexusUrl url);
    }
}
