// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Threading.Tasks;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.API;

namespace ParkitectNexus.Data.Assets
{
    /// <summary>
    ///     Provides the functionality of an online Parkitect asset storage.
    /// </summary>
    public interface IRemoteAssetRepository
    {
        Task<IDownloadedAsset> DownloadAsset(ApiAsset asset);
    }
}
