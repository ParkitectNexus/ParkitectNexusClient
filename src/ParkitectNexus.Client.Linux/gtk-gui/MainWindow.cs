
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
    private global::Gtk.UIManager UIManager;
    
    private global::Gtk.Action FileAction;
    
    private global::Gtk.Action DonateAction;
    
    private global::Gtk.Action helpAction;
    
    private global::Gtk.Action AboutAction;
    
    private global::Gtk.Action ParkitectFolderAction;
    
    private global::Gtk.Action ConfigAction;
    
    private global::Gtk.VBox vbox1;
    
    private global::Gtk.MenuBar menubar1;
    
    private global::Gtk.HBox hbox2;
    
    private global::Gtk.Alignment alignment4;
    
    private global::Gtk.Image ParkitectnexusLogo;
    
    private global::Gtk.Notebook Pages;
    
    private global::Gtk.Label label4;
    
    private global::Gtk.HBox hbox1;
    
    private global::Gtk.Alignment alignment1;
    
    private global::Gtk.Button btnLaunchParkitect;

    protected virtual void Build ()
    {
        global::Stetic.Gui.Initialize (this);
        // Widget MainWindow
        this.UIManager = new global::Gtk.UIManager ();
        global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
        this.FileAction = new global::Gtk.Action ("FileAction", global::Mono.Unix.Catalog.GetString ("File"), null, null);
        this.FileAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("File");
        w1.Add (this.FileAction, null);
        this.DonateAction = new global::Gtk.Action ("DonateAction", global::Mono.Unix.Catalog.GetString ("Donate"), null, null);
        this.DonateAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Donate");
        w1.Add (this.DonateAction, null);
        this.helpAction = new global::Gtk.Action ("helpAction", global::Mono.Unix.Catalog.GetString ("Help"), null, "gtk-help");
        this.helpAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Help");
        w1.Add (this.helpAction, null);
        this.AboutAction = new global::Gtk.Action ("AboutAction", global::Mono.Unix.Catalog.GetString ("About"), null, null);
        this.AboutAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("About");
        w1.Add (this.AboutAction, null);
        this.ParkitectFolderAction = new global::Gtk.Action ("ParkitectFolderAction", global::Mono.Unix.Catalog.GetString ("Parkitect folder"), null, null);
        this.ParkitectFolderAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Parkitect folder");
        w1.Add (this.ParkitectFolderAction, null);
        this.ConfigAction = new global::Gtk.Action ("ConfigAction", global::Mono.Unix.Catalog.GetString ("Config"), null, null);
        this.ConfigAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Config");
        w1.Add (this.ConfigAction, null);
        this.UIManager.InsertActionGroup (w1, 0);
        this.AddAccelGroup (this.UIManager.AccelGroup);
        this.Name = "MainWindow";
        this.Title = global::Mono.Unix.Catalog.GetString ("Parkitect Nexus");
        this.Icon = global::Gdk.Pixbuf.LoadFromResource ("ParkitectNexus.Client.Linux.parkitectnexus_logo.png");
        this.WindowPosition = ((global::Gtk.WindowPosition)(4));
        // Container child MainWindow.Gtk.Container+ContainerChild
        this.vbox1 = new global::Gtk.VBox ();
        this.vbox1.Name = "vbox1";
        this.vbox1.Spacing = 6;
        // Container child vbox1.Gtk.Box+BoxChild
        this.UIManager.AddUiFromString ("<ui><menubar name='menubar1'><menu name='helpAction' action='helpAction'><menuitem name='AboutAction' action='AboutAction'/></menu></menubar></ui>");
        this.menubar1 = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menubar1")));
        this.menubar1.Name = "menubar1";
        this.vbox1.Add (this.menubar1);
        global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.menubar1]));
        w2.Position = 0;
        w2.Expand = false;
        w2.Fill = false;
        // Container child vbox1.Gtk.Box+BoxChild
        this.hbox2 = new global::Gtk.HBox ();
        this.hbox2.Name = "hbox2";
        this.hbox2.Spacing = 6;
        // Container child hbox2.Gtk.Box+BoxChild
        this.alignment4 = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
        this.alignment4.Name = "alignment4";
        this.alignment4.LeftPadding = ((uint)(10));
        this.alignment4.TopPadding = ((uint)(5));
        // Container child alignment4.Gtk.Container+ContainerChild
        this.ParkitectnexusLogo = new global::Gtk.Image ();
        this.ParkitectnexusLogo.Name = "ParkitectnexusLogo";
        this.ParkitectnexusLogo.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("ParkitectNexus.Client.Linux.parkitectnexus_logo_full.png");
        this.alignment4.Add (this.ParkitectnexusLogo);
        this.hbox2.Add (this.alignment4);
        global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.alignment4]));
        w4.Position = 0;
        w4.Expand = false;
        w4.Fill = false;
        this.vbox1.Add (this.hbox2);
        global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox2]));
        w5.Position = 1;
        w5.Expand = false;
        w5.Fill = false;
        // Container child vbox1.Gtk.Box+BoxChild
        this.Pages = new global::Gtk.Notebook ();
        this.Pages.CanFocus = true;
        this.Pages.Name = "Pages";
        this.Pages.CurrentPage = 0;
        this.Pages.BorderWidth = ((uint)(3));
        // Notebook tab
        global::Gtk.Label w6 = new global::Gtk.Label ();
        w6.Visible = true;
        this.Pages.Add (w6);
        this.label4 = new global::Gtk.Label ();
        this.label4.Name = "label4";
        this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("Mods");
        this.Pages.SetTabLabel (w6, this.label4);
        this.label4.ShowAll ();
        this.vbox1.Add (this.Pages);
        global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.Pages]));
        w7.Position = 2;
        // Container child vbox1.Gtk.Box+BoxChild
        this.hbox1 = new global::Gtk.HBox ();
        this.hbox1.Name = "hbox1";
        this.hbox1.Spacing = 6;
        // Container child hbox1.Gtk.Box+BoxChild
        this.alignment1 = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
        this.alignment1.Name = "alignment1";
        this.hbox1.Add (this.alignment1);
        global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.alignment1]));
        w8.Position = 0;
        // Container child hbox1.Gtk.Box+BoxChild
        this.btnLaunchParkitect = new global::Gtk.Button ();
        this.btnLaunchParkitect.WidthRequest = 97;
        this.btnLaunchParkitect.HeightRequest = 34;
        this.btnLaunchParkitect.CanFocus = true;
        this.btnLaunchParkitect.Name = "btnLaunchParkitect";
        this.btnLaunchParkitect.UseUnderline = true;
        this.btnLaunchParkitect.Relief = ((global::Gtk.ReliefStyle)(1));
        global::Gtk.Image w9 = new global::Gtk.Image ();
        w9.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("ParkitectNexus.Client.Linux.parkitect_logo.png");
        this.btnLaunchParkitect.Image = w9;
        this.hbox1.Add (this.btnLaunchParkitect);
        global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.btnLaunchParkitect]));
        w10.Position = 1;
        w10.Expand = false;
        w10.Fill = false;
        w10.Padding = ((uint)(5));
        this.vbox1.Add (this.hbox1);
        global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox1]));
        w11.Position = 3;
        w11.Expand = false;
        w11.Fill = false;
        this.Add (this.vbox1);
        if ((this.Child != null)) {
            this.Child.ShowAll ();
        }
        this.DefaultWidth = 460;
        this.DefaultHeight = 370;
        this.Show ();
        this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
        this.AboutAction.Activated += new global::System.EventHandler (this.AboutDialog);
        this.btnLaunchParkitect.Clicked += new global::System.EventHandler (this.LaunchParkitect);
    }
}