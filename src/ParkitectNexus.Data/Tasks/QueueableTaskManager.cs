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
        private readonly List<IQueueableTask> _runningTasks = new List<IQueueableTask>();
        private readonly Queue<IQueueableTask> _tasks = new Queue<IQueueableTask>();
        private CancellationTokenSource _cancellationTokenSource;
        private IQueueableTask _currentTask;
        public event EventHandler<QueueableTaskEventArgs> TaskAdded;
        public event EventHandler<QueueableTaskEventArgs> TaskRemoved;
        public event EventHandler<QueueableTaskEventArgs> TaskFinished;

        public int Count => _tasks.Count + (_currentTask != null ? 1 : 0);

        public void Add(IQueueableTask task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            _tasks.Enqueue(task);
            OnTaskAdded(new QueueableTaskEventArgs(task));

            RunNext();
        }

        public void ClearCompleted()
        {
            var completed =
                _runningTasks.Where(t => t.Status == TaskStatus.Canceled || t.Status == TaskStatus.Stopped).ToArray();

            foreach (var item in completed)
            {
                _runningTasks.Remove(item);
                OnTaskRemoved(new QueueableTaskEventArgs(item));
            }
        }

        private void RunNext()
        {
            if (!_tasks.Any()) return;
            if (_currentTask != null) return;

            _currentTask = _tasks.Dequeue();
            _runningTasks.Add(_currentTask);

            if (_currentTask.Status != TaskStatus.Queued)
            {
                RunNext();
                return;
            }

            _currentTask.StatusChanged += Task_StatusChanged;

            _cancellationTokenSource = new CancellationTokenSource();
            _currentTask.Run(_cancellationTokenSource.Token);
        }

        private void Task_StatusChanged(object sender, EventArgs e)
        {
            if (_currentTask.Status != TaskStatus.Running)
            {
                var task = _currentTask;
                task.StatusChanged -= Task_StatusChanged;
                _currentTask = null;

                OnTaskFinished(new QueueableTaskEventArgs(task));
                RunNext();
            }
        }

        protected virtual void OnTaskAdded(QueueableTaskEventArgs e)
        {
            TaskAdded?.Invoke(this, e);
        }

        protected virtual void OnTaskRemoved(QueueableTaskEventArgs e)
        {
            TaskRemoved?.Invoke(this, e);
        }

        protected virtual void OnTaskFinished(QueueableTaskEventArgs e)
        {
            TaskFinished?.Invoke(this, e);
        }
    }
}
