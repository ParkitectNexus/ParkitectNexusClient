// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Data.Tasks
{
    public class QueueableTaskEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QueueableTaskEventArgs" /> class.
        /// </summary>
        /// <param name="task">The task.</param>
        public QueueableTaskEventArgs(IQueueableTask task)
        {
            Task = task;
        }

        /// <summary>
        ///     Gets the task.
        /// </summary>
        public IQueueableTask Task { get; }
    }
}