using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web.Models;

namespace ParkitectNexus.Data.Tasks.Prefab
{
    public class UpdateModTask : QueueableTask
    {
        private readonly IModAsset _mod;
        private readonly IParkitect _parkitect;
        private readonly IAssetUpdatesManager _assetUpdatesManager;
        private readonly IQueueableTaskManager _queueableTaskManager;

        public UpdateModTask(IModAsset mod, IParkitect parkitect, IAssetUpdatesManager assetUpdatesManager,
            IQueueableTaskManager queueableTaskManager)
        {
            if (mod == null) throw new ArgumentNullException(nameof(mod));
            _mod = mod;
            _parkitect = parkitect;
            _assetUpdatesManager = assetUpdatesManager;
            _queueableTaskManager = queueableTaskManager;

            Name = $"Update mod {mod.Name}";
        }

        #region Overrides of QueueableTask

        public async override Task Run(CancellationToken token)
        {
            UpdateStatus("Checking latest version...", 25, TaskStatus.Running);

            var updateAvailable = await _assetUpdatesManager.IsUpdateAvailableOnline(_mod);

            if (!updateAvailable)
            {
                UpdateStatus("No update available.", 100, TaskStatus.Finished);
                return;
            }

            UpdateStatus("Deleting mod...", 50, TaskStatus.Running);
            _parkitect.Assets.DeleteAsset(_mod);
            await Task.Delay(500, token);

            var task = ObjectFactory.GetInstance<InstallAssetTask>();
            task.Data = new InstallUrlAction(_mod.Id);
            _queueableTaskManager.Add(task);

            UpdateStatus("Finished checking for updates.", 100, TaskStatus.Finished);
        }

        #endregion
    }
}
