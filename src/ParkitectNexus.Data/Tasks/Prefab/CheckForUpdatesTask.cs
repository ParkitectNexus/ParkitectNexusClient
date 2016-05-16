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
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.API;
using ParkitectNexus.Data.Web.Models;

namespace ParkitectNexus.Data.Tasks.Prefab
{
    public class CheckForUpdatesTask : QueueableTask
    {
        private readonly IParkitect _parkitect;
        private readonly IWebsite _website;
        private readonly ILogger _log;
        private readonly IQueueableTaskManager _queueableTaskManager;

        public CheckForUpdatesTask(IParkitect parkitect, IWebsite website, ILogger log, IQueueableTaskManager queueableTaskManager) : base("Check for updates")
        {
            _parkitect = parkitect;
            _website = website;
            _log = log;
            _queueableTaskManager = queueableTaskManager;
            StatusDescription = "Check for updates.";
        }

        #region Overrides of QueueableTask

        public override async Task Run(CancellationToken token)
        {
            var count = 0;
            UpdateStatus("Gathering information...", 0, TaskStatus.Running);

            foreach (var required in await _website.API.GetRequiredModIdentifiers())
            {
                if (_parkitect.Assets[AssetType.Mod].All(m => m.Id != required))
                {
                    // Create install task for the dependency.
                    var installDependencyTask = ObjectFactory.GetInstance<InstallAssetTask>();
                    installDependencyTask.Data = new InstallUrlAction(required);

                    // Insert the install task in the queueable task manager.
                    _queueableTaskManager.Add(installDependencyTask);
                }
            }

            var mods = _parkitect.Assets[AssetType.Mod].OfType<IModAsset>().ToArray();

            for (var index = 0; index < mods.Length; index++)
            {
                var mod = mods[index];
                if (!mod.Information.IsDevelopment || mod.Id == null)
                    continue;

                UpdateStatus($"{index + 1}/{mods.Length}: Checking mod: {mod.Name}...", (int)(index * (100.0f/mods.Length)), TaskStatus.Running);

                try
                {
                    var info = await _website.API.GetAsset(mod.Id);
                    if (mod.Tag != info.Resource.Data.Version)
                    {
                        _queueableTaskManager.With(mod).Add<UpdateModTask>();
                        count++;
                    }
                }
                catch (Exception e)
                {
                    _log.WriteLine("Failed to gather mod info: " + e.Message);
                }
            }

            UpdateStatus($"Found {count} available updates.", 100, TaskStatus.Finished);
        }

        #endregion
    }
}
