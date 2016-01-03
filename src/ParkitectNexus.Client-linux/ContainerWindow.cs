using System;

namespace ParkitectNexus.Client.GTK
{
    public partial class ContainerWindow : Gtk.Window
    {
        public ContainerWindow () :
            base (Gtk.WindowType.Toplevel)
        {
            this.Build ();
        }
    }
}

