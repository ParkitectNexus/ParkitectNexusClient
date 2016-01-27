// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Threading;
using System.Threading.Tasks;
using ParkitectNexus.AssetMagic.Elements;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.Models;

namespace ParkitectNexus.Data.Tasks.Prefab
{
    public class InstallAssetTask : UrlQueueableTask
    {
        private readonly IParkitect _parkitect;
        private readonly IParkitectNexusWebsite _website;
        private readonly IRemoteAssetRepository _remoteAssetRepository;

        public InstallAssetTask(IParkitect parkitect, IParkitectNexusWebsite website, IRemoteAssetRepository remoteAssetRepository)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (website == null) throw new ArgumentNullException(nameof(website));
            if (remoteAssetRepository == null) throw new ArgumentNullException(nameof(remoteAssetRepository));
            _parkitect = parkitect;
            _website = website;
            _remoteAssetRepository = remoteAssetRepository;

            Name = "Install asset";
        }

        #region Overrides of QueueableTask

        public async override Task Run(CancellationToken token)
        {
            var data = Data as InstallUrlAction;
            if (data == null)
            {
                UpdateStatus("Invalid URL supplied.", 100, TaskStatus.Canceled);
                return;
            }

            UpdateStatus("Fetching asset information...", 0, TaskStatus.Running);

            var assetData = await _website.API.GetAsset(data.Id);

            UpdateStatus($"Installing {assetData.Type.ToString().ToLower()} '{assetData.Name}'...", 15, TaskStatus.Running);
            var downloadedAsset = await _remoteAssetRepository.DownloadAsset(assetData);

            await _parkitect.LocalAssets.StoreAsset(downloadedAsset);

            UpdateStatus($"Installed {assetData.Type.ToString().ToLower()} '{assetData.Name}'.", 100, TaskStatus.Stopped);
        }

        #endregion
    }
}
