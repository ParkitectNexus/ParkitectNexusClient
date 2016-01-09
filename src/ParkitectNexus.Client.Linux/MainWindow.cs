using System;
using Gtk;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Client.Linux;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data;
using ParkitectNexus.Client;
using ParkitectNexus.Data.Game;

public partial class MainWindow: Gtk.Window, IPresenter
{
    private readonly IParkitect _parkitect;

    public MainWindow (IPresenterFactory presenterFactory,IParkitect parkitect,IPathResolver pathResolver, ILogger logger) : base (Gtk.WindowType.Toplevel)
    {
        _parkitect = parkitect;

        Build ();
        logger.Open(System.IO.Path.Combine(pathResolver.AppData(), "ParkitectNexusLauncher.log"));

        presenterFactory.InstantiatePresenter<ParkitectInstallDialog> (this);
        ModLoaderUtil.InstallModLoader (parkitect);

        //remove the default page
        Pages.RemovePage (0);

        AddPageToPages("Mods", presenterFactory.InstantiatePresenter<ModsPage> ());

    }

    private void AddPageToPages(string text,Widget page)
    {
        Label label = new global::Gtk.Label ();
        label = new global::Gtk.Label ();
        label.Name = text;
        label.LabelProp = global::Mono.Unix.Catalog.GetString (text);

        Pages.Add (page);
        Pages.SetTabLabel (page,label);

        label.ShowAll ();
        page.ShowAll ();
    }

    protected void OnDeleteEvent (object sender, DeleteEventArgs a)
    {
        Application.Quit ();
        a.RetVal = true;
    }
        
    protected void LaunchParkitect (object sender, EventArgs e)
    {
        //launch parkitect and close the application
        _parkitect.Launch ();
        Environment.Exit (0);
    }
}
