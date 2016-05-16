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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.Models;

namespace ParkitectNexus.Data.Tasks.Prefab
{
    public class InstallAssetTask : UrlQueueableTask
    {
        private const bool InstallOptionalDependencies = true;

        private readonly IParkitect _parkitect;
        private readonly IQueueableTaskManager _queueableTaskManager;
        private readonly IRemoteAssetRepository _remoteAssetRepository;
        private readonly IWebsite _website;

        public InstallAssetTask(IParkitect parkitect, IWebsite website, IRemoteAssetRepository remoteAssetRepository,
            IQueueableTaskManager queueableTaskManager) : base("Install asset")
        {
            _parkitect = parkitect;
            _website = website;
            _remoteAssetRepository = remoteAssetRepository;
            _queueableTaskManager = queueableTaskManager;

            StatusDescription = "Install an asset.";
        }

        #region Overrides of QueueableTask

        public override async Task Run(CancellationToken token)
        {
            try
            {
                // Cast the data instance to InstallUrlAction.
                var data = Data as InstallUrlAction;
                if (data == null)
                {
                    UpdateStatus("Invalid URL supplied.", 100, TaskStatus.Canceled);
                    return;
                }

                // Fetch asset information from the PN API.
                UpdateStatus("Fetching asset information...", 0, TaskStatus.Running);

                var assetData = await _website.API.GetAsset(data.Id);

                if (assetData.Type == AssetType.Mod && assetData.Resource.Data.Version == null)
                {
                    UpdateStatus($"The '{assetData.Name}' mod has not yet been released! Ask the author to release it.", 100, TaskStatus.Canceled);
                    return;
                }

                // Download the asset trough the RemoveAssetRepository.
                UpdateStatus($"Installing {assetData.Type.ToString().ToLower()} '{assetData.Name}'...", 15,
                    TaskStatus.Running);

                var downloadedAsset = await _remoteAssetRepository.DownloadAsset(assetData);

                // Store the asset in in the appropriate location.
                var asset = await _parkitect.Assets.StoreAsset(downloadedAsset);

                // If the downloaded asset is a mod, add a "compile mod" task to the queue.
                var modAsset = asset as IModAsset;
                if (modAsset != null)
                    _queueableTaskManager.With(modAsset).InsertAfter<CompileModTask>(this);

                // Ensure dependencies have been installed.
                foreach (var dependency in assetData.Dependencies.Data)
                {
//                    if (!dependency.Required && !InstallOptionalDependencies)
//                        continue;

                    if (_parkitect.Assets.Any(a => a.Id == dependency.Id))
                        continue;

                    // Create install task for the dependency.
                    var installDependencyTask = ObjectFactory.GetInstance<InstallAssetTask>();
                    installDependencyTask.Data = new InstallUrlAction(dependency.Id);

                    // Insert the install task in the queueable task manager.
                    _queueableTaskManager.InsertAfter(installDependencyTask, this);
                }


                // Update the status of the task.
                UpdateStatus($"Installed {assetData.Type.ToString().ToLower()} '{assetData.Name}'.", 100,
                    TaskStatus.Finished);
            }
            catch (Exception e)
            {
                UpdateStatus($"Installation failed. {e.Message}", 100, TaskStatus.FinishedWithErrors);
            }
        }

        #endregion
    }
}
