// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Threading;
using System.Threading.Tasks;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Tasks
{
    public abstract class QueueableTask : IQueueableTask
    {
        private readonly ILogger _log;
        protected QueueableTask(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            Name = name;

            _log = ObjectFactory.GetInstance<ILogger>();
        }

        protected virtual void UpdateStatus(string description, int completion, TaskStatus status)
        {
            StatusDescription = description;
            Status = status;
            CompletionPercentage = Math.Max(0, Math.Min(completion, 100));

            _log.WriteLine($"Task status {status} {CompletionPercentage}%: {description}",
                status == TaskStatus.Canceled || status == TaskStatus.FinishedWithErrors
                    ? LogLevel.Error
                    : LogLevel.Debug);

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
                _log.WriteLine("Task was canceled.", LogLevel.Info);

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
