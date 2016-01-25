using System;

namespace ParkitectNexus.Data.Tasks
{
    public interface IQueueableTaskManager
    {
        event EventHandler<QueueableTaskEventArgs> TaskAdded;
        event EventHandler<QueueableTaskEventArgs> TaskRemoved;
        void Add(IQueueableTask task);
        void ClearCompleted();
    }
}