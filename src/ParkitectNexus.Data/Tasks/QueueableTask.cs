// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Tasks
{
    public abstract class QueueableTask : IQueueableTask
    {
        protected virtual void UpdateStatus(string description, int completion, TaskStatus status)
        {
            StatusDescription = description;
            Status = status;
            CompletionPercentage = Math.Max(0, Math.Min(completion, 100));

            OnStatusChanged();
        }

        protected virtual void OnStatusChanged()
        {
            StatusChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void ThrowIfCancellationRequested(CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                Status = TaskStatus.Canceled;
                token.ThrowIfCancellationRequested();
            }
        }

        #region Implementation of IQueueableTask

        public event EventHandler StatusChanged;

        public virtual string Name { get; protected set; }

        public virtual string StatusDescription { get; set; }

        public virtual TaskStatus Status { get; set; }

        public virtual int CompletionPercentage { get; set; }

        public abstract Task Run(CancellationToken token);

        #endregion
    }
}