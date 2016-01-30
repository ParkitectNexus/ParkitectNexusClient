using System;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Caching;
using ParkitectNexus.Data.Tasks;
using Gtk;

namespace ParkitectNexus.Client.Linux
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class TaskList : Gtk.Bin, IPresenter, IPage
	{
		private NodeStore _tasks = new NodeStore(typeof(TreeNodeTaskContainer));
		private IQueueableTaskManager _queueTaskManager;

		public TaskList (IQueueableTaskManager taskManager)
		{
			_queueTaskManager = taskManager;


			_queueTaskManager.TaskAdded += (object sender, QueueableTaskEventArgs e) => {
				
				e.Task.StatusChanged += (object sender_status, EventArgs e_status) => {
					var task = (IQueueableTask)sender_status;

				};
			};
			this.Build ();
		}

		public void OnOpen()
		{
		}
		public void OnClose()
		{
		}
	}
}

