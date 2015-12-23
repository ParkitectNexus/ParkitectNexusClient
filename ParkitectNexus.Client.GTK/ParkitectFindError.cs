using System;
using Gtk;
using System.IO;
using System.Linq;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Client.GTK
{
	public partial class ParkitectFindError : Gtk.Dialog
	{
		private IParkitect parkitect;
		public ParkitectFindError (IParkitect parkitect)
		{
			this.parkitect = parkitect;
			this.Build ();
		}

		protected void Cancel (object sender, EventArgs e)
		{
			//close the application
			Environment.Exit (0);
		}

		protected void SelectDirectory (object sender, EventArgs e)
		{

			Gtk.FileChooserDialog fc = new FileChooserDialog ("Parkitect Nexus", this,Gtk.FileChooserAction.SelectFolder,"Cancle", Gtk.ResponseType.Cancel,"Select",  Gtk.ResponseType.Ok);
			fc.Show ();


			fc.SelectionChanged += (o, args) => {
				fc.StyleGetProperty("Select");
			};

			while (true) {
				switch (fc.Run ()) {
				case (int)Gtk.ResponseType.Cancel:
					fc.Destroy ();
					break;
				case (int)Gtk.ResponseType.Ok:
					string s = fc.Filename;
					if (!this.parkitect.SetInstallationPathIfValid (fc.Filename)) {
						Gtk.MessageDialog errorDialog = new MessageDialog (fc, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.YesNo, "the folder you selected does not contain Parkitect!\n Would you like to try again?");
						if (errorDialog.Run () == (int)Gtk.ResponseType.No) {
							Environment.Exit (0);
						} else {
							errorDialog.Destroy ();
						}
					} else {
					}
					break;
				}
			}

			//throw new NotImplementedException ();
		}

	}
}

