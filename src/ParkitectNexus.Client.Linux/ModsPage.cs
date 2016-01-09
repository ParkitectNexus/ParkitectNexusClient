using System;
using ParkitectNexus.Data.Presenter;

namespace ParkitectNexus.Client.Linux
{
    [System.ComponentModel.ToolboxItem (true)]
    public partial class ModsPage : Gtk.Bin, IPresenter
    {
        public ModsPage ()
        {
            this.Build ();
        }

        protected void InstallMod (object sender, EventArgs e)
        {
            throw new NotImplementedException ();
        }

        protected void CheckForUpdatesForMod (object sender, EventArgs e)
        {
            throw new NotImplementedException ();
        }

        protected void UninstallMod (object sender, EventArgs e)
        {
            throw new NotImplementedException ();
        }

        protected void VistModWebsite (object o, Gtk.ButtonPressEventArgs args)
        {
            throw new NotImplementedException ();
        }
    }
}

