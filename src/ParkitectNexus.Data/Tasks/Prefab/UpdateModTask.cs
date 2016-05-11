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
        private readonly IAssetUpdatesManager _assetUpdatesManager;
        private readonly IModAsset _mod;
        private readonly IParkitect _parkitect;
        private readonly IQueueableTaskManager _queueableTaskManager;

        public UpdateModTask(IModAsset mod, IParkitect parkitect, IAssetUpdatesManager assetUpdatesManager,
            IQueueableTaskManager queueableTaskManager) : base($"Update mod {mod?.Name}")
        {
            if (mod == null) throw new ArgumentNullException(nameof(mod));
            _mod = mod;
            _parkitect = parkitect;
            _assetUpdatesManager = assetUpdatesManager;
            _queueableTaskManager = queueableTaskManager;

            StatusDescription = $"Update mod {mod.Name}";
        }

        #region Overrides of QueueableTask

        public override async Task Run(CancellationToken token)
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
