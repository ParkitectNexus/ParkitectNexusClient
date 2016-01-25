using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Tasks;
using TaskStatus = ParkitectNexus.Data.Tasks.TaskStatus;

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

            Text = "Tasks";

            TestWaitQueueableTask t1;
            _taskManager.Add(t1 = new TestWaitQueueableTask(1));
            _taskManager.Add(new TestWaitQueueableTask(2));

            t1.StatusChanged += (sender, args) =>
            {
                if (t1.Status == TaskStatus.Stopped)
                    _taskManager.Add(new TestWaitQueueableTask(3));
            };
        }

        private void _taskManager_TaskRemoved(object sender, QueueableTaskEventArgs e)
        {
            //
        }

        private void _taskManager_TaskAdded(object sender, QueueableTaskEventArgs e)
        {
            var count = Controls.OfType<TaskUserControl>().Count();
            var control = new TaskUserControl(e.Task);

            control.Dock = DockStyle.Top;

            Controls.Add(control);
        }
    }
}
