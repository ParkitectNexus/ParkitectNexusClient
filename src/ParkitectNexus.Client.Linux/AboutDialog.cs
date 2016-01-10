using System;
using ParkitectNexus.Data.Presenter;

namespace ParkitectNexus.Client.Linux
{
    public partial class AboutDialog : Gtk.Dialog, IPresenter
    {
        public AboutDialog ()
        {
            this.Build ();

        }
    }
}

