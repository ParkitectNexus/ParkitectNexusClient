using System;
using Gtk;
using System.Collections;
using System.Collections.Generic;
using ParkitectNexus.Mod.ModLoader;

public partial class MainWindow: Gtk.Window
{
	public class Mod
	{
		public Mod(bool active, string name)
		{
			this.name = name;
			this.isActive = active;
		}
		public string name{ get; set;}
		public bool isActive{ get; set;}
	}

	private ListStore mods = new ListStore(typeof(Mod));

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();

		Gtk.TreeViewColumn isModEnabled = new TreeViewColumn ();
		isModEnabled.PackStart (new Gtk.CellRendererToggle (), false);

		Gtk.TreeViewColumn nameOfMod = new TreeViewColumn ();
		var nameOfModRenderer = new Gtk.CellRendererText ();
		nameOfMod.PackStart (nameOfModRenderer, true);

		isModEnabled.SetCellDataFunc (nameOfModRenderer, new TreeCellDataFunc (RenderNameOfMod));

		Mods.AppendColumn (isModEnabled);
		Mods.AppendColumn (nameOfMod);

		mods.AppendValues (new Mod (true, "mod1"));

		Mods.Model = mods;


	
	}

	private void RenderNameOfMod(Gtk.TreeViewColumn columsn, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
	{
		Mod mod = (Mod) model.GetValue (iter, 0);

		(cell as Gtk.CellRendererText).Text = mod.name;
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void Start (object sender, EventArgs e)
	{
		var checkbox = new global::Gtk.Button ();
		checkbox.Label = global::Mono.Unix.Catalog.GetString ("GtkButton");


	}


}
