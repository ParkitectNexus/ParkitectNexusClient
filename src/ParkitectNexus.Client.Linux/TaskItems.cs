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
              private MainWindow _mainwindow;     

            public TaskItems (IQueueableTaskManager taskManager,IPresenter parentWindow)
			{
			    this.Build ();
                
                _mainwindow = (MainWindow)parentWindow;
				_queueTaskManager = taskManager;
				
                Tasks.AppendColumn ("Name", new CellRendererText (), "text", 0).Resizable = true;
                Tasks.AppendColumn ("Status", new CellRendererText (), "text", 1).Resizable = true;
                Tasks.AppendColumn ("Description", new CellRendererText (), "text", 2).Resizable = true;
                Tasks.AppendColumn ("Percent", new CellRendererProgress (), "value", 3).Resizable = true;

				Tasks.ShowAll ();

				_queueTaskManager.TaskAdded += (object sender, QueueableTaskEventArgs e) => {

                    _tasks.AddNode(new TreeNodeTaskContainer(e.Task,Tasks));
                    UpdateTaskCount();
				};

                _queueTaskManager.TaskFinished += (object sender, QueueableTaskEventArgs e) => 
                {
                    UpdateTaskCount();
                };
		
				Tasks.NodeStore = _tasks;
				
			}

            private void UpdateTaskCount()
            {
                int count = _queueTaskManager.Count;
                _mainwindow.ChangeLabelText(this, count == 0 ? "Tasks" : $"Tasks ({count})"); 
            }

			public void OnOpen()
			{
			}
			public void OnClose()
			{
			}
	}

}

