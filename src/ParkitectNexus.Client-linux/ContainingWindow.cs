using System;

namespace ParkitectNexus.Client.GTK
{
    public partial class ContainingWindow : Gtk.Window
    {
        public ContainingWindow () :
            base (Gtk.WindowType.Toplevel)
        {
            this.Build ();
        }
    }
}

