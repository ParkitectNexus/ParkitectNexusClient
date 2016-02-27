// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Threading;
using System.Threading.Tasks;
using ParkitectNexus.Data.Assets;

namespace ParkitectNexus.Data.Tasks.Prefab
{
    public class CheckForUpdatesTask : QueueableTask
    {
        private readonly IAssetUpdatesManager _assetUpdatesManager;

        public CheckForUpdatesTask(IAssetUpdatesManager assetUpdatesManager) : base ("Updates check")
        {
            _assetUpdatesManager = assetUpdatesManager;
        }

        #region Overrides of QueueableTask

        public async override Task Run(CancellationToken token)
        {
            UpdateStatus("Checking for updates...", 25, TaskStatus.Running);
            var count = await _assetUpdatesManager.CheckForUpdates();

            UpdateStatus($"Found {count} available updates.", 100, TaskStatus.Finished);
        }

        #endregion
    }
}
