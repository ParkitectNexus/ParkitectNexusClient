using System;

namespace ParkitectNexus.Client.GTK
{
    public partial class IntialConfiguration : Gtk.ActionGroup
    {
        public IntialConfiguration () :
            base ("ParkitectNexus.Client.GTK.IntialConfiguration")
        {
            this.Build ();
        }
    }
}

