// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ParkitectNexus.Data.Tasks
{
    public class QueueableTaskManager : IQueueableTaskManager
    {
        private readonly List<IQueueableTask> _runningAndFinishedTasks = new List<IQueueableTask>();
        private readonly Queue<IQueueableTask> _queuedTasks = new Queue<IQueueableTask>();

        private CancellationTokenSource _cancellationTokenSource;
        private IQueueableTask _currentTask;

        /// <summary>
        /// Occurs when a task was added.
        /// </summary>
        public event EventHandler<QueueableTaskEventArgs> TaskAdded;

        /// <summary>
        /// Occurs when task was removed.
        /// </summary>
        public event EventHandler<QueueableTaskEventArgs> TaskRemoved;

        /// <summary>
        /// Occurs when a task has finished.
        /// </summary>
        public event EventHandler<QueueableTaskEventArgs> TaskFinished;

        /// <summary>
        /// Gets the task count.
        /// </summary>
        public int Count => _queuedTasks.Count + (_currentTask != null ? 1 : 0);

        /// <summary>
        /// Gets a value indicating whether this instance is idle.
        /// </summary>
        public bool IsIdle => Count == 0;

        /// <summary>
        /// Adds the specified task to the queue.
        /// </summary>
        /// <param name="task">The task.</param>
        public void Add(IQueueableTask task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            _queuedTasks.Enqueue(task);
            OnTaskAdded(new QueueableTaskEventArgs(task));

            RunNext();
        }

        /// <summary>
        /// Clears the completed tasks from the tasks list.
        /// </summary>
        public void ClearCompleted()
        {
            // Gather a collection of finished tasks.
            var completed = _runningAndFinishedTasks
                .Where(t => t.Status == TaskStatus.Canceled || t.Status == TaskStatus.Finished)
                .ToArray();

            // Remove every finished task from the list and raise the TaskRemoved event.
            foreach (var item in completed)
            {
                _runningAndFinishedTasks.Remove(item);
                OnTaskRemoved(new QueueableTaskEventArgs(item));
            }
        }

        private async void RunNext()
        {
            // Do not start new tasks if there are none queued.
            if (!_queuedTasks.Any()) return;

            // Do not start new tasks if there's already a running task.
            if (_currentTask != null) return;

            // Store the first task in the queue in _currentTask and add it to the running tasks list.
            _currentTask = _queuedTasks.Dequeue();
            _runningAndFinishedTasks.Add(_currentTask);

            // Ensure the dequeued task is awaiting being run.
            if (_currentTask.Status != TaskStatus.Queued && _currentTask.Status != TaskStatus.Break)
            {
                RunNext();
                return;
            }

            // Create a cancellation token source for this task.
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await _currentTask.Run(_cancellationTokenSource.Token);

                // If the task requests a break, move it to the end of the queue.
                if (_currentTask.Status == TaskStatus.Break)
                {
                    _runningAndFinishedTasks.Remove(_currentTask);
                    Add(_currentTask);
                }
            }
            catch (Exception e)
            {
                // Find the deepest inner exception to extract the error message from.
                while (e is AggregateException)
                    e = ((AggregateException) e).InnerException;

                // Set the task status to "canceled" and change the status description to the error message.
                _currentTask.Status = TaskStatus.Canceled;
                _currentTask.CompletionPercentage = 100;
                _currentTask.StatusDescription = $"Failed: {e.Message}";
            }
            finally
            {
                // After the task has either successfully run or failed, a next task can be started.
                CurrentTaskFinished();
                RunNext();
            }
        }

        private void CurrentTaskFinished()
        {
            // Remove the event listener and unset the "current task" value.
            var task = _currentTask;
            _currentTask = null;

            // Raise the TaskFinished event.
            OnTaskFinished(new QueueableTaskEventArgs(task));
        }

        /// <summary>
        /// Raises the <see cref="E:TaskAdded" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ParkitectNexus.Data.Tasks.QueueableTaskEventArgs" /> instance containing the event data.</param>
        protected virtual void OnTaskAdded(QueueableTaskEventArgs e)
        {
            TaskAdded?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:TaskRemoved" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ParkitectNexus.Data.Tasks.QueueableTaskEventArgs" /> instance containing the event data.</param>
        protected virtual void OnTaskRemoved(QueueableTaskEventArgs e)
        {
            TaskRemoved?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:TaskFinished" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ParkitectNexus.Data.Tasks.QueueableTaskEventArgs" /> instance containing the event data.</param>
        protected virtual void OnTaskFinished(QueueableTaskEventArgs e)
        {
            TaskFinished?.Invoke(this, e);
        }
    }
}
