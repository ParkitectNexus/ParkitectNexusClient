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