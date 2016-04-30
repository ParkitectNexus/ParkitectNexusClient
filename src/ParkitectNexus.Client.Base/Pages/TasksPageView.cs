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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using ParkitectNexus.Client.Base.Components;
using ParkitectNexus.Client.Base.Main;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Game.Windows;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Tasks;
using ParkitectNexus.Data.Tasks.Prefab;
using Xwt;
using Xwt.Drawing;
using OperatingSystem = ParkitectNexus.Data.Utilities.OperatingSystem;

namespace ParkitectNexus.Client.Base.Pages
{
    public class TasksPageView : ScrollView, IPresenter, IPageView
    {
        private Type[] _tasksOfInterest =
        {
            typeof(CompileModTask),
            typeof(InstallAssetTask)
        };

        private readonly Label _nothingLabel;
        private readonly IQueueableTaskManager _taskManager;
        private readonly VBox _vBox;
        private string _displayName = "Tasks";
        private readonly MainView _mainView;
        private List<TaskView> _taskViews = new List<TaskView>();

        public TasksPageView(IQueueableTaskManager taskManager, IPresenter parent)
        {

            if (!(parent is MainView))
                throw new ArgumentException("parent must be MainView", nameof(parent));

            _mainView = (MainView)parent;

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

        public void HandleSizeChange()
        {
            // Stupid fix awgh...
            if (OperatingSystem.Detect() == SupportedOperatingSystem.MacOSX)
            {
                var backendHost = BackendHost as WidgetBackendHost;
                backendHost?.OnVisibleRectChanged();
            }
        }

        private void TaskManager_TaskFinished(object sender, QueueableTaskEventArgs e)
        {
            var count = _taskManager.Count;
            DisplayName = count == 0 ? "Tasks" : $"Tasks ({count})";
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

                if (_tasksOfInterest.Contains(e.Task.GetType()))
                {
                    _mainView.SwitchToTab(4);

                    if (OperatingSystem.Detect() == SupportedOperatingSystem.Windows)
                        User32.SetForegroundWindow(Process.GetCurrentProcess().MainWindowHandle);
                }
                ReOrderTasks();
            });
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
