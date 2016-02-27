using System;
using ParkitectNexus.Data.Tasks;
using Gtk;

namespace ParkitectNexus.Client.Linux
{
	[Gtk.TreeNode (ListOnly=true)]
	public class TreeNodeTaskContainer: Gtk.TreeNode
	{
		private IQueueableTask _task;
		private NodeView _nodeView;

		public TreeNodeTaskContainer (IQueueableTask task,NodeView view)
		{
			_nodeView = view;
			_task = task;

			_task.StatusChanged += (object sender, EventArgs e) => {
				Gtk.Application.Invoke (delegate {
				_nodeView.QueueDraw();
				});
			};
		}

		[Gtk.TreeNodeValue(Column=0)]
		public string TaskName{ 
			get{ 
				return _task.Name;
			}
		}

		[Gtk.TreeNodeValue(Column=1)]
		public string TaskDescription{ 
			get{ 
				return _task.StatusDescription;
			}
		}

		[Gtk.TreeNodeValue(Column=2)]
		public string Status{ 
			get{ 
				switch (_task.Status) { 
				case  TaskStatus.Queued:
					return "Queued";
				case TaskStatus.Running:
					return "Running";
                    case TaskStatus.Finished:
					return "Stopped";
				case TaskStatus.Canceled:
					return "Canceled";

				}
				return "ummm ... something went wrong";

			}
		}

		[Gtk.TreeNodeValue(Column=3)]
		public float Percent{ 
			get{ 
				
				if(_task.CompletionPercentage >= 100)
				{
					return 100;
				}
				return _task.CompletionPercentage;
			}
		}

	}
}

