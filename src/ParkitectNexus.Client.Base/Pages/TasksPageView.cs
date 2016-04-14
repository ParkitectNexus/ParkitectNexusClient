// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Linq;
using ParkitectNexus.Client.Base.Components;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Tasks;
using Xwt;
using Xwt.Drawing;

namespace ParkitectNexus.Client.Base.Pages
{
    public class TasksPageView : ScrollView, IPresenter, IPageView
    {
        private readonly Label _nothingLabel;
        private readonly IQueueableTaskManager _taskManager;
        private readonly VBox _vBox;
        private string _displayName = "Tasks";
        private List<TaskView> _taskViews = new List<TaskView>();

        public TasksPageView(IQueueableTaskManager taskManager)
        {
            _taskManager = taskManager;

            taskManager.TaskAdded += TaskManager_TaskAdded;
            taskManager.TaskRemoved += TaskManager_TaskRemoved;
            taskManager.TaskFinished += TaskManager_TaskFinished;

            _vBox = new VBox();

            _nothingLabel = new Label("All tasks have been completed. Yay!")
            {
                TextAlignment = Alignment.Center,
                Font = Font.SystemFont.WithStyle(FontStyle.Italic).WithWeight(FontWeight.Light)
            };
            _vBox.PackStart(_nothingLabel, true, true);

            Content = _vBox;
        }

        private void TaskManager_TaskFinished(object sender, QueueableTaskEventArgs e)
        {
            var count = _taskManager.Count;
            DisplayName = count == 0 ? "Tasks" : $"Tasks ({count})";
        }

        private void ReOrderTasks()
        {
            _vBox.Clear();

            _taskViews =
                _taskViews.Select(v => new KeyValuePair<int, TaskView>(_taskManager.IndexOf(v.Task), v))
                    .Where(kv => kv.Key >= 0)
                    .Select(kv => kv.Value)
                    .ToList();

            foreach (var v in _taskViews)
                _vBox.PackStart(v);

            if (!_taskViews.Any())
                _vBox.PackStart(_nothingLabel, true, true);
        }

        private void TaskManager_TaskRemoved(object sender, QueueableTaskEventArgs e)
        {
            var count = _taskManager.Count;
            DisplayName = count == 0 ? "Tasks" : $"Tasks ({count})";

            var tv = _taskViews.FirstOrDefault(v => v.Task == e.Task);
            if (tv == null)
                return;

            Application.Invoke(() =>
            {
                _taskViews.Remove(tv);
                _vBox.Remove(tv);
            });
        }

        private void TaskManager_TaskAdded(object sender, QueueableTaskEventArgs e)
        {
            var count = _taskManager.Count;
            DisplayName = count == 0 ? "Tasks" : $"Tasks ({count})";

            if (count == 1)
            {
                _taskViews.Clear();
                _taskManager.ClearCompleted();
            }
            Application.Invoke(() =>
            {
                var tv = new TaskView(e.Task);
                _taskViews.Add(tv);
                ReOrderTasks();
            });
        }

        protected virtual void OnDisplayNameChanged()
        {
            DisplayNameChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Implementation of IPageView

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (_displayName == value) return;
                _displayName = value;
                OnDisplayNameChanged();
            }
        }

        public event EventHandler DisplayNameChanged;

        #endregion
    }
}
