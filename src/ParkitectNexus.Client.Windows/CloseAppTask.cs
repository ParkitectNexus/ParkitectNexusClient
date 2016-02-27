// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Threading;
using System.Threading.Tasks;
using ParkitectNexus.Data.Tasks;
using TaskStatus = ParkitectNexus.Data.Tasks.TaskStatus;

namespace ParkitectNexus.Client.Windows
{
    public class CloseAppTask : QueueableTask
    {
        private readonly MainForm _form;

        public CloseAppTask(MainForm form) : base("Close application")
        {
            if (form == null) throw new ArgumentNullException(nameof(form));
            _form = form;
            
            StatusDescription = "Close the ParkitectNexus Client.";
        }

        #region Overrides of QueueableTask

        public override Task Run(CancellationToken token)
        {
            UpdateStatus("Closing the ParkitectNexus Client", 25, TaskStatus.Running);
            _form.Close();

            UpdateStatus("Closed the ParkitectNexus Client", 100, TaskStatus.Finished);
            return Task.FromResult(0);
        }

        #endregion
    }
}
