
// This file has been generated by the GUI designer. Do not modify.
namespace ParkitectNexus.Client.Linux
{
	public partial class TaskItems
	{
		private global::Gtk.VBox vbox2;
		
		private global::Gtk.ScrolledWindow scrolledwindow1;
		
		private global::Gtk.NodeView Tasks;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget ParkitectNexus.Client.Linux.TaskItems
			global::Stetic.BinContainer.Attach (this);
			this.Name = "ParkitectNexus.Client.Linux.TaskItems";
			// Container child ParkitectNexus.Client.Linux.TaskItems.Gtk.Container+ContainerChild
			this.vbox2 = new global::Gtk.VBox ();
			this.vbox2.Name = "vbox2";
			this.vbox2.Spacing = 6;
			// Container child vbox2.Gtk.Box+BoxChild
			this.scrolledwindow1 = new global::Gtk.ScrolledWindow ();
			this.scrolledwindow1.CanFocus = true;
			this.scrolledwindow1.Name = "scrolledwindow1";
			this.scrolledwindow1.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child scrolledwindow1.Gtk.Container+ContainerChild
			this.Tasks = new global::Gtk.NodeView ();
			this.Tasks.CanFocus = true;
			this.Tasks.Name = "Tasks";
			this.scrolledwindow1.Add (this.Tasks);
			this.vbox2.Add (this.scrolledwindow1);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.scrolledwindow1]));
			w2.Position = 0;
			this.Add (this.vbox2);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
		}
	}
}