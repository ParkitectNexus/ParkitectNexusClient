// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Tasks.Prefab
{
    public class TestWaitQueueableTask : QueueableTask
    {
        public TestWaitQueueableTask(int number)
        {
            Name = $"Wait task for testing #{number}";
        }

        #region Overrides of QueueableTask

        public override async Task Run(CancellationToken token)
        {
            for (var i = 0; i < 100; i++)
            {
                ThrowIfCancellationRequested(token);

                UpdateStatus("Such descriptive, much wow" + new string(Enumerable.Repeat('.', i/10).ToArray()), i,
                    TaskStatus.Running);
                await Task.Delay(100, token);
            }

            UpdateStatus("Such completion, much wow", 100, TaskStatus.Stopped);
        }

        #endregion
    }
}