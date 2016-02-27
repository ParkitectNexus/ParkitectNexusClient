// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Threading;
using System.Threading.Tasks;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Data.Tasks.Prefab
{
    public class LaunchGameTask : QueueableTask
    {
        private readonly IParkitect _parkitect;

        public LaunchGameTask(IParkitect parkitect) : base("Launch parkitect")
        {
            _parkitect = parkitect;
        }

        #region Overrides of QueueableTask

        public override Task Run(CancellationToken token)
        {
            UpdateStatus("Launching Parkitect...", 25, TaskStatus.Running);
            _parkitect.Launch();

            UpdateStatus("Launched Parkitect", 100, TaskStatus.Finished);
            return Task.FromResult(0);
        }

        #endregion
    }
}
