using System;

namespace ParkitectNexus.Data.Tasks
{
    public interface IQueueableTaskManager
    {
        event EventHandler<QueueableTaskEventArgs> TaskAdded;
        event EventHandler<QueueableTaskEventArgs> TaskRemoved;
        event EventHandler<QueueableTaskEventArgs> TaskFinished;
        int Count { get; }
        void Add(IQueueableTask task);
        void ClearCompleted();
    }
}
