using System;
using Gtk;
using ParkitectNexus.Data.Tasks;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Tasks.Prefab;
using Gdk;

namespace ParkitectNexus.Client.Linux
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class TaskItems : Gtk.Bin,IPresenter, IPage
	{
			private NodeStore _tasks = new NodeStore(typeof(TreeNodeTaskContainer));
			private IQueueableTaskManager _queueTaskManager;

			public TaskItems (IQueueableTaskManager taskManager)
			{
			this.Build ();

				_queueTaskManager = taskManager;
				
				Tasks.AppendColumn ("Name", new CellRendererText (), "text", 0);
				Tasks.AppendColumn ("Status", new CellRendererText (), "text", 1);
				Tasks.AppendColumn ("Description", new CellRendererText (), "text", 2);
				Tasks.AppendColumn ("Percent", new CellRendererProgress (), "value", 3);


				Tasks.ShowAll ();

				_queueTaskManager.TaskAdded += (object sender, QueueableTaskEventArgs e) => {

                    _tasks.AddNode(new TreeNodeTaskContainer(e.Task,Tasks));
				};
		
				Tasks.NodeStore = _tasks;
				
			}

			public void OnOpen()
			{
			}
			public void OnClose()
			{
			}
	}

}

