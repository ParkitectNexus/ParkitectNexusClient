using System;
using Gtk;
using System.IO;
using System.Linq;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Client.GTK
{
	public partial class ParkitectFindError : Gtk.Dialog
	{
		private IParkitect _parkitect;
		public ParkitectFindError (IParkitect parkitect)
		{
			this._parkitect = parkitect;
			this.Build ();
		}

		/// <summary>
		/// cancel the application and respond with a cancle response
		/// </summary>
		protected void Cancel (object sender, EventArgs e)
		{
			//close the application
			this.Respond(ResponseType.Close);
		}

		/// <summary>
		/// Select the director for parkitect
		/// </summary>
		protected void SelectDirectory (object sender, EventArgs e)
		{

			Gtk.FileChooserDialog fc = new FileChooserDialog ("Parkitect Nexus", this,Gtk.FileChooserAction.SelectFolder,"Cancle", Gtk.ResponseType.Cancel,"Select",  Gtk.ResponseType.Ok);
			fc.Show ();


			fc.SelectionChanged += (o, args) => {
				fc.StyleGetProperty("Select");
			};
			bool ask_user_again = true;
			while (ask_user_again) {
				
				switch (fc.Run ()) {
				case (int)Gtk.ResponseType.Cancel:
					ask_user_again = false;
					break;
				case (int)Gtk.ResponseType.Ok:
					if (!this._parkitect.SetInstallationPathIfValid (fc.Filename)) {
						Gtk.MessageDialog errorDialog = new MessageDialog (fc, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.YesNo, "the folder you selected does not contain Parkitect!\n Would you like to try again?");
						switch (errorDialog.Run ()) {
						case (int)ResponseType.Yes:
							errorDialog.Destroy ();
						break;
						default:
							Environment.Exit (0);
							break;
						}
					} else {
						this.Respond (ResponseType.Ok);
						ask_user_again = false;
					}
				break;
				 default:
					Environment.Exit (0);
				break;
					
				}

			}
			fc.Destroy ();

			//throw new NotImplementedException ();
		}

	}
}

