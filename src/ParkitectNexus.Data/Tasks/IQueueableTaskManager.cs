// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Data.Tasks
{
    public interface IQueueableTaskManager
    {
        int Count { get; }

        event EventHandler<QueueableTaskEventArgs> TaskAdded;

        event EventHandler<QueueableTaskEventArgs> TaskRemoved;

        event EventHandler<QueueableTaskEventArgs> TaskFinished;

        void Add(IQueueableTask task);
        void ClearCompleted();
    }
}