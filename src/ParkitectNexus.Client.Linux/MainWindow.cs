using System;
using Gtk;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Client.Linux;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Client;
using ParkitectNexus.Data.Game;

public partial class MainWindow: Gtk.Window, IPresenter
{
    private readonly IParkitect _parkitect;
    private readonly IPresenterFactory _presenterFactory;
    private IPresenter[] _presenterPages;
    private int _previousPage = -1;

    public MainWindow (IPresenterFactory presenterFactory,IParkitect parkitect, ILogger logger) : base (Gtk.WindowType.Toplevel)
    {
        _parkitect = parkitect;
        _presenterFactory = presenterFactory;

        Build ();
        logger.Open(System.IO.Path.Combine(AppData.Path, "ParkitectNexusLauncher.log"));


        presenterFactory.InstantiatePresenter<ParkitectInstallDialog> (this);
        ModLoaderUtil.InstallModLoader (parkitect,logger);
       presenterFactory.InstantiatePresenter<ProtocalInstallUtility>();

        _presenterPages = new IPresenter[] { presenterFactory.InstantiatePresenter<ModsPage>(this), presenterFactory.InstantiatePresenter<SavegamePage>(this), presenterFactory.InstantiatePresenter<BlueprintPage>(this) };

        //remove the default page
        Pages.RemovePage (0);

        AddPageToPages("Mods", (Widget)_presenterPages[0]);
        AddPageToPages("Savegames", (Widget)_presenterPages[1]);
        AddPageToPages("Blueprints", (Widget)_presenterPages[2]);

        Pages.SwitchPage += Pages_SwitchPage;

    }

    private void Pages_SwitchPage(object o, SwitchPageArgs args)
    {
        if (args.PageNum >= 0 && _presenterPages.Length > args.PageNum)
        {
            if(_previousPage != -1)
            ((IPage)_presenterPages[_previousPage]).OnClose();
            _previousPage = (int)args.PageNum;
            ((IPage)_presenterPages[args.PageNum]).OnOpen();
        }
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
    protected void AboutDialog (object sender, EventArgs e)
    {
        var aboutDialog = _presenterFactory.InstantiatePresenter<ParkitectNexus.Client.Linux.AboutDialog> ();
        aboutDialog.Run ();
        aboutDialog.Destroy ();
    }

}
