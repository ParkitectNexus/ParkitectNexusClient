using System;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Client.GTK
{
	[Gtk.TreeNode (ListOnly=true)]
	public class TreeNodeModContainer : Gtk.TreeNode
	{
		private IParkitectMod _parkitectMod;
		public TreeNodeModContainer(IParkitectMod parkitectMod)
		{
			this._parkitectMod = parkitectMod;
			AvaliableVersion= "-";
		}

		public IParkitectMod GetParkitectMod
		{
			get{ return _parkitectMod; }
		}


		[Gtk.TreeNodeValue(Column=3)]
		public string AvaliableVersion{ get; set;}

		[Gtk.TreeNodeValue (Column=2)]
		public string Version
		{ 
			get
			{ 
				return _parkitectMod.Tag;
			}
		}

		[Gtk.TreeNodeValue (Column=1)]
		public string Name
		{ 
			get
			{ 
				return _parkitectMod.Name;
			}
		}

		[Gtk.TreeNodeValue (Column=0)]
		public bool IsActive{ 
			get{ 
				return _parkitectMod.IsEnabled;
			} 
			set
			{ 
				_parkitectMod.IsEnabled = value; 
				_parkitectMod.Save ();
			}
		}
	}
}

