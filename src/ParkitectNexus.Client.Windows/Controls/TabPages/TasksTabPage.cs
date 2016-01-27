using System;
using System.Windows.Forms;
using MetroFramework.Controls;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Tasks;

namespace ParkitectNexus.Client.Windows.Controls.TabPages
{
    public class TasksTabPage : MetroTabPage, IPresenter
    {
        private readonly IQueueableTaskManager _taskManager;

        public TasksTabPage(IQueueableTaskManager taskManager)
        {
            if (taskManager == null) throw new ArgumentNullException(nameof(taskManager));

            _taskManager = taskManager;

            _taskManager.TaskAdded += _taskManager_TaskAdded;
            _taskManager.TaskRemoved += _taskManager_TaskRemoved;
            _taskManager.TaskFinished += _taskManager_TaskFinished;
            Text = "Tasks";
        }

        private void _taskManager_TaskFinished(object sender, QueueableTaskEventArgs e)
        {
            UpdateTasksCount();
        }

        private void _taskManager_TaskRemoved(object sender, QueueableTaskEventArgs e)
        {
            //
        }

        private void _taskManager_TaskAdded(object sender, QueueableTaskEventArgs e)
        {
            var control = new TaskUserControl(e.Task) {Dock = DockStyle.Top};
            Controls.Add(control);
            UpdateTasksCount();
        }

        private void UpdateTasksCount()
        {
            var count = _taskManager.Count;

            Text = count == 0 ? "Tasks" : $"Tasks ({count})";
        }
    }
}
