// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Controls;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Tasks;

namespace ParkitectNexus.Client.Windows.Controls.TabPages
{
    public class TasksTabPage : MetroTabPage, IPresenter
    {
        private readonly IQueueableTaskManager _taskManager;
        private readonly MetroLabel _noTasksLabel;

        public TasksTabPage(IQueueableTaskManager taskManager)
        {
            if (taskManager == null) throw new ArgumentNullException(nameof(taskManager));

            _taskManager = taskManager;

            _taskManager.TaskAdded += _taskManager_TaskAdded;
            _taskManager.TaskRemoved += _taskManager_TaskRemoved;
            _taskManager.TaskFinished += _taskManager_TaskFinished;

            AutoScroll = true;
            Text = "Tasks";

            _noTasksLabel = new MetroLabel
            {
                Location = new Point(6, 6),
                AutoSize = true,
                Text = "There are currently no running tasks.",
                Enabled = true
            };

            Controls.Add(_noTasksLabel);
        }

        private void _taskManager_TaskFinished(object sender, QueueableTaskEventArgs e)
        {
            UpdateTasksCount();
        }

        private void _taskManager_TaskRemoved(object sender, QueueableTaskEventArgs e)
        {
            var taskControl = Controls.OfType<TaskUserControl>().FirstOrDefault(c => c.Task == e.Task);

            if (taskControl == null)
                return;

            Controls.Remove(taskControl);
            ReorderTasks();
        }

        private void _taskManager_TaskAdded(object sender, QueueableTaskEventArgs e)
        {
            if (_noTasksLabel.Enabled)
            {
                Controls.Remove(_noTasksLabel);
                _noTasksLabel.Enabled = false;
            }

            var control = new TaskUserControl(e.Task)
            {
                Width = HScroll ? Width - HorizontalScrollbarSize : Width
            };

            control.Top = control.Height*_taskManager.IndexOf(e.Task);

            Controls.Add(control);
            UpdateTasksCount();
            ReorderTasks();
        }

        private void ReorderTasks()
        {
            foreach (var taskControl in Controls.OfType<TaskUserControl>())
                taskControl.Top = _taskManager.IndexOf(taskControl.Task)*taskControl.Height;
        }

        #region Overrides of Panel

        /// <summary>
        /// Fires the event indicating that the panel has been resized. Inheriting controls should use this in favor of actually listening to the event, but should still call base.onResize to ensure that the event is fired for external listeners.
        /// </summary>
        /// <param name="eventargs">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
        protected override void OnResize(EventArgs eventargs)
        {
            foreach (var control in Controls.OfType<TaskUserControl>())
                control.Width = HScroll ? Width - HorizontalScrollbarSize : Width;

            base.OnResize(eventargs);
        }

        #endregion

        private void UpdateTasksCount()
        {
            var count = _taskManager.Count;

            Text = count == 0 ? "Tasks" : $"Tasks ({count})";
        }
    }
}
