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
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.API;
using ParkitectNexus.Data.Web.Models;

namespace ParkitectNexus.Data.Tasks.Prefab
{
    public class UpdateModTask : QueueableTask
    {
        private readonly IModAsset _mod;
        private readonly IParkitect _parkitect;
        private readonly IQueueableTaskManager _queueableTaskManager;
        private readonly IWebsite _website;

        public UpdateModTask(IModAsset mod, IParkitect parkitect, IQueueableTaskManager queueableTaskManager, IWebsite website) : base($"Update mod {mod?.Name}")
        {
            if (mod == null) throw new ArgumentNullException(nameof(mod));
            _mod = mod;
            _parkitect = parkitect;
            _queueableTaskManager = queueableTaskManager;
            _website = website;

            StatusDescription = $"Update mod {mod.Name}";
        }

        #region Overrides of QueueableTask

        public override async Task Run(CancellationToken token)
        {
            UpdateStatus("Checking latest version...", 25, TaskStatus.Running);

            var info = await _website.API.GetAsset(_mod.Id);
            if (_mod.Tag == info.Resource.Data.Version)
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

            UpdateStatus("Started installation task.", 100, TaskStatus.Finished);
        }

        #endregion
    }
}
