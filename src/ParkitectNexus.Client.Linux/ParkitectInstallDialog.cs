using System;
using ParkitectNexus.Data.Presenter;
using Gtk;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Client.Linux
{
    /// <summary>
    /// Parkitect install dialog.
    /// </summary>
    public class ParkitectInstallDialog : IPresenter
    {

        public ParkitectInstallDialog (IPresenter parent,IParkitect parkitect) 
        {
            if (!parkitect.IsInstalled) {
                MessageDialog parkitectNotFoundDialog = new MessageDialog ((Window)parent, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.OkCancel, "We couldn't detect Parkitect on your machine Please point me to it!");

                switch (parkitectNotFoundDialog.Run ()) {
                case (int)ResponseType.Ok:

                    FileChooserDialog fc = new FileChooserDialog ("Parkitect Nexus", parkitectNotFoundDialog, Gtk.FileChooserAction.SelectFolder, "Cancle", Gtk.ResponseType.Cancel, "Select", Gtk.ResponseType.Ok);
                    fc.Show ();


                    fc.SelectionChanged += (o, args) => {
                        fc.StyleGetProperty ("Select");
                    };
                    bool ask_user_again = true;
                    while (ask_user_again) {

                        switch (fc.Run ()) {
                        case (int)Gtk.ResponseType.Cancel:
                            ask_user_again = false;
                            break;
                        case (int)Gtk.ResponseType.Ok:
                            if (!parkitect.SetInstallationPathIfValid (fc.Filename)) {
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
                                ask_user_again = false;
                            }
                            break;
                        default:
                            Environment.Exit (0);
                            break;

                        }

                    }
                    fc.Destroy ();
                    break;
                default:
                    Environment.Exit (0);
                    break;
                }
                parkitectNotFoundDialog.Destroy ();
            }
        }
    }
}

