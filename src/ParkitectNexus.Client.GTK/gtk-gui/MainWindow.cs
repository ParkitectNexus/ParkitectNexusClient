
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.UIManager UIManager;
	
	private global::Gtk.Action helpAction;
	
	private global::Gtk.Action helpAction1;
	
	private global::Gtk.Action aboutAction;
	
	private global::Gtk.Action infoAction;
	
	private global::Gtk.VBox vbox3;
	
	private global::Gtk.MenuBar menubar2;
	
	private global::Gtk.Image image2;
	
	private global::Gtk.HPaned hpaned4;
	
	private global::Gtk.VBox vbox2;
	
	private global::Gtk.Button button56;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow;
	
	private global::Gtk.NodeView Mods;
	
	private global::Gtk.VBox vbox6;
	
	private global::Gtk.Frame frame1;
	
	private global::Gtk.Alignment GtkAlignment2;
	
	private global::Gtk.VBox vbox4;
	
	private global::Gtk.HBox hbox6;
	
	private global::Gtk.Label label9;
	
	private global::Gtk.Label Mod_Name;
	
	private global::Gtk.HBox hbox7;
	
	private global::Gtk.Label label11;
	
	private global::Gtk.Label Mod_Version;
	
	private global::Gtk.HBox hbox8;
	
	private global::Gtk.Label label13;
	
	private global::Gtk.Button Mod_Website;
	
	private global::Gtk.HSeparator hseparator3;
	
	private global::Gtk.Label Mov_Development_Status;
	
	private global::Gtk.Button Mod_Update;
	
	private global::Gtk.Button Mod_Uninstall;
	
	private global::Gtk.Label GtkLabel6;
	
	private global::Gtk.HBox hbox3;
	
	private global::Gtk.VBox vbox5;
	
	private global::Gtk.Button button1500;
	
	private global::Gtk.Button button1499;
	
	private global::Gtk.Alignment alignment5;
	
	private global::Gtk.VBox vbox7;
	
	private global::Gtk.Button button1523;
	
	private global::Gtk.HSeparator hseparator2;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.UIManager = new global::Gtk.UIManager ();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
		this.helpAction = new global::Gtk.Action ("helpAction", global::Mono.Unix.Catalog.GetString ("help"), null, null);
		this.helpAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("help");
		w1.Add (this.helpAction, null);
		this.helpAction1 = new global::Gtk.Action ("helpAction1", global::Mono.Unix.Catalog.GetString ("help"), null, null);
		this.helpAction1.ShortLabel = global::Mono.Unix.Catalog.GetString ("help");
		w1.Add (this.helpAction1, null);
		this.aboutAction = new global::Gtk.Action ("aboutAction", global::Mono.Unix.Catalog.GetString ("about"), null, null);
		this.aboutAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("about");
		w1.Add (this.aboutAction, null);
		this.infoAction = new global::Gtk.Action ("infoAction", global::Mono.Unix.Catalog.GetString ("info"), null, null);
		this.infoAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("info");
		w1.Add (this.infoAction, null);
		this.UIManager.InsertActionGroup (w1, 0);
		this.AddAccelGroup (this.UIManager.AccelGroup);
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		this.BorderWidth = ((uint)(3));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox3 = new global::Gtk.VBox ();
		this.vbox3.Name = "vbox3";
		this.vbox3.Spacing = 6;
		// Container child vbox3.Gtk.Box+BoxChild
		this.UIManager.AddUiFromString ("<ui><menubar name='menubar2'/></ui>");
		this.menubar2 = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menubar2")));
		this.menubar2.Name = "menubar2";
		this.vbox3.Add (this.menubar2);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.menubar2]));
		w2.Position = 0;
		w2.Expand = false;
		w2.Fill = false;
		// Container child vbox3.Gtk.Box+BoxChild
		this.image2 = new global::Gtk.Image ();
		this.image2.Name = "image2";
		this.image2.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("ParkitectNexus.Client.GTK.dialog_banner.png");
		this.vbox3.Add (this.image2);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.image2]));
		w3.Position = 1;
		w3.Expand = false;
		w3.Fill = false;
		// Container child vbox3.Gtk.Box+BoxChild
		this.hpaned4 = new global::Gtk.HPaned ();
		this.hpaned4.CanFocus = true;
		this.hpaned4.Name = "hpaned4";
		this.hpaned4.Position = 303;
		// Container child hpaned4.Gtk.Paned+PanedChild
		this.vbox2 = new global::Gtk.VBox ();
		this.vbox2.Name = "vbox2";
		this.vbox2.Spacing = 6;
		// Container child vbox2.Gtk.Box+BoxChild
		this.button56 = new global::Gtk.Button ();
		this.button56.CanFocus = true;
		this.button56.Name = "button56";
		this.button56.UseUnderline = true;
		this.button56.Label = global::Mono.Unix.Catalog.GetString ("Install Mod");
		this.vbox2.Add (this.button56);
		global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.button56]));
		w4.Position = 0;
		w4.Expand = false;
		w4.Fill = false;
		// Container child vbox2.Gtk.Box+BoxChild
		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
		this.Mods = new global::Gtk.NodeView ();
		this.Mods.CanFocus = true;
		this.Mods.Name = "Mods";
		this.GtkScrolledWindow.Add (this.Mods);
		this.vbox2.Add (this.GtkScrolledWindow);
		global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.GtkScrolledWindow]));
		w6.Position = 1;
		this.hpaned4.Add (this.vbox2);
		global::Gtk.Paned.PanedChild w7 = ((global::Gtk.Paned.PanedChild)(this.hpaned4 [this.vbox2]));
		w7.Resize = false;
		// Container child hpaned4.Gtk.Paned+PanedChild
		this.vbox6 = new global::Gtk.VBox ();
		this.vbox6.Name = "vbox6";
		this.vbox6.Spacing = 6;
		// Container child vbox6.Gtk.Box+BoxChild
		this.frame1 = new global::Gtk.Frame ();
		this.frame1.Name = "frame1";
		this.frame1.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame1.Gtk.Container+ContainerChild
		this.GtkAlignment2 = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
		this.GtkAlignment2.Name = "GtkAlignment2";
		this.GtkAlignment2.LeftPadding = ((uint)(12));
		// Container child GtkAlignment2.Gtk.Container+ContainerChild
		this.vbox4 = new global::Gtk.VBox ();
		this.vbox4.Name = "vbox4";
		this.vbox4.Spacing = 6;
		// Container child vbox4.Gtk.Box+BoxChild
		this.hbox6 = new global::Gtk.HBox ();
		this.hbox6.Name = "hbox6";
		this.hbox6.Spacing = 6;
		// Container child hbox6.Gtk.Box+BoxChild
		this.label9 = new global::Gtk.Label ();
		this.label9.Name = "label9";
		this.label9.LabelProp = global::Mono.Unix.Catalog.GetString ("Mod:");
		this.hbox6.Add (this.label9);
		global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox6 [this.label9]));
		w8.Position = 0;
		w8.Expand = false;
		w8.Fill = false;
		// Container child hbox6.Gtk.Box+BoxChild
		this.Mod_Name = new global::Gtk.Label ();
		this.Mod_Name.Name = "Mod_Name";
		this.Mod_Name.LabelProp = global::Mono.Unix.Catalog.GetString ("label10");
		this.hbox6.Add (this.Mod_Name);
		global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox6 [this.Mod_Name]));
		w9.Position = 2;
		w9.Expand = false;
		w9.Fill = false;
		this.vbox4.Add (this.hbox6);
		global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.hbox6]));
		w10.Position = 0;
		w10.Expand = false;
		w10.Fill = false;
		// Container child vbox4.Gtk.Box+BoxChild
		this.hbox7 = new global::Gtk.HBox ();
		this.hbox7.Name = "hbox7";
		this.hbox7.Spacing = 6;
		// Container child hbox7.Gtk.Box+BoxChild
		this.label11 = new global::Gtk.Label ();
		this.label11.Name = "label11";
		this.label11.LabelProp = global::Mono.Unix.Catalog.GetString ("Version:");
		this.hbox7.Add (this.label11);
		global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox7 [this.label11]));
		w11.Position = 0;
		w11.Expand = false;
		w11.Fill = false;
		// Container child hbox7.Gtk.Box+BoxChild
		this.Mod_Version = new global::Gtk.Label ();
		this.Mod_Version.Name = "Mod_Version";
		this.Mod_Version.LabelProp = global::Mono.Unix.Catalog.GetString ("label10");
		this.hbox7.Add (this.Mod_Version);
		global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.hbox7 [this.Mod_Version]));
		w12.Position = 2;
		w12.Expand = false;
		w12.Fill = false;
		this.vbox4.Add (this.hbox7);
		global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.hbox7]));
		w13.Position = 1;
		w13.Expand = false;
		w13.Fill = false;
		// Container child vbox4.Gtk.Box+BoxChild
		this.hbox8 = new global::Gtk.HBox ();
		this.hbox8.Name = "hbox8";
		this.hbox8.Spacing = 6;
		// Container child hbox8.Gtk.Box+BoxChild
		this.label13 = new global::Gtk.Label ();
		this.label13.Name = "label13";
		this.label13.LabelProp = global::Mono.Unix.Catalog.GetString ("Website:");
		this.hbox8.Add (this.label13);
		global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.hbox8 [this.label13]));
		w14.Position = 0;
		w14.Expand = false;
		w14.Fill = false;
		// Container child hbox8.Gtk.Box+BoxChild
		this.Mod_Website = new global::Gtk.Button ();
		this.Mod_Website.CanFocus = true;
		this.Mod_Website.Name = "Mod_Website";
		this.Mod_Website.UseUnderline = true;
		this.Mod_Website.Relief = ((global::Gtk.ReliefStyle)(2));
		this.Mod_Website.Label = global::Mono.Unix.Catalog.GetString ("View on ParkitectNexus");
		this.hbox8.Add (this.Mod_Website);
		global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.hbox8 [this.Mod_Website]));
		w15.Position = 2;
		w15.Expand = false;
		w15.Fill = false;
		this.vbox4.Add (this.hbox8);
		global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.hbox8]));
		w16.Position = 2;
		w16.Expand = false;
		w16.Fill = false;
		// Container child vbox4.Gtk.Box+BoxChild
		this.hseparator3 = new global::Gtk.HSeparator ();
		this.hseparator3.Name = "hseparator3";
		this.vbox4.Add (this.hseparator3);
		global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.hseparator3]));
		w17.Position = 3;
		w17.Expand = false;
		w17.Fill = false;
		w17.Padding = ((uint)(15));
		// Container child vbox4.Gtk.Box+BoxChild
		this.Mov_Development_Status = new global::Gtk.Label ();
		this.Mov_Development_Status.Name = "Mov_Development_Status";
		this.Mov_Development_Status.LabelProp = global::Mono.Unix.Catalog.GetString ("MOD IN DEVELOPMENT");
		this.vbox4.Add (this.Mov_Development_Status);
		global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.Mov_Development_Status]));
		w18.Position = 4;
		w18.Expand = false;
		w18.Fill = false;
		w18.Padding = ((uint)(5));
		// Container child vbox4.Gtk.Box+BoxChild
		this.Mod_Update = new global::Gtk.Button ();
		this.Mod_Update.CanFocus = true;
		this.Mod_Update.Name = "Mod_Update";
		this.Mod_Update.UseUnderline = true;
		this.Mod_Update.Label = global::Mono.Unix.Catalog.GetString ("Check for Updates");
		this.vbox4.Add (this.Mod_Update);
		global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.Mod_Update]));
		w19.Position = 5;
		w19.Expand = false;
		w19.Fill = false;
		// Container child vbox4.Gtk.Box+BoxChild
		this.Mod_Uninstall = new global::Gtk.Button ();
		this.Mod_Uninstall.CanFocus = true;
		this.Mod_Uninstall.Name = "Mod_Uninstall";
		this.Mod_Uninstall.UseUnderline = true;
		this.Mod_Uninstall.Label = global::Mono.Unix.Catalog.GetString ("Uninstall");
		this.vbox4.Add (this.Mod_Uninstall);
		global::Gtk.Box.BoxChild w20 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.Mod_Uninstall]));
		w20.Position = 6;
		w20.Expand = false;
		w20.Fill = false;
		this.GtkAlignment2.Add (this.vbox4);
		this.frame1.Add (this.GtkAlignment2);
		this.GtkLabel6 = new global::Gtk.Label ();
		this.GtkLabel6.Name = "GtkLabel6";
		this.GtkLabel6.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>Options</b>");
		this.GtkLabel6.UseMarkup = true;
		this.frame1.LabelWidget = this.GtkLabel6;
		this.vbox6.Add (this.frame1);
		global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.vbox6 [this.frame1]));
		w23.Position = 0;
		w23.Expand = false;
		w23.Fill = false;
		this.hpaned4.Add (this.vbox6);
		this.vbox3.Add (this.hpaned4);
		global::Gtk.Box.BoxChild w25 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hpaned4]));
		w25.Position = 2;
		// Container child vbox3.Gtk.Box+BoxChild
		this.hbox3 = new global::Gtk.HBox ();
		this.hbox3.Name = "hbox3";
		this.hbox3.Spacing = 6;
		// Container child hbox3.Gtk.Box+BoxChild
		this.vbox5 = new global::Gtk.VBox ();
		this.vbox5.Name = "vbox5";
		this.vbox5.Spacing = 6;
		// Container child vbox5.Gtk.Box+BoxChild
		this.button1500 = new global::Gtk.Button ();
		this.button1500.CanFocus = true;
		this.button1500.Name = "button1500";
		this.button1500.UseUnderline = true;
		this.button1500.Relief = ((global::Gtk.ReliefStyle)(2));
		this.button1500.Label = global::Mono.Unix.Catalog.GetString ("Visit ParkitectNexus");
		this.vbox5.Add (this.button1500);
		global::Gtk.Box.BoxChild w26 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.button1500]));
		w26.Position = 0;
		w26.Expand = false;
		w26.Fill = false;
		// Container child vbox5.Gtk.Box+BoxChild
		this.button1499 = new global::Gtk.Button ();
		this.button1499.CanFocus = true;
		this.button1499.Name = "button1499";
		this.button1499.UseUnderline = true;
		this.button1499.Relief = ((global::Gtk.ReliefStyle)(2));
		this.button1499.Xalign = 0F;
		this.button1499.Label = global::Mono.Unix.Catalog.GetString ("Donate");
		this.vbox5.Add (this.button1499);
		global::Gtk.Box.BoxChild w27 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.button1499]));
		w27.Position = 1;
		w27.Expand = false;
		w27.Fill = false;
		this.hbox3.Add (this.vbox5);
		global::Gtk.Box.BoxChild w28 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.vbox5]));
		w28.Position = 0;
		w28.Expand = false;
		w28.Fill = false;
		w28.Padding = ((uint)(5));
		// Container child hbox3.Gtk.Box+BoxChild
		this.alignment5 = new global::Gtk.Alignment (0.5F, 0.5F, 1F, 1F);
		this.alignment5.Name = "alignment5";
		this.hbox3.Add (this.alignment5);
		global::Gtk.Box.BoxChild w29 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.alignment5]));
		w29.Position = 1;
		// Container child hbox3.Gtk.Box+BoxChild
		this.vbox7 = new global::Gtk.VBox ();
		this.vbox7.Name = "vbox7";
		this.vbox7.Spacing = 6;
		// Container child vbox7.Gtk.Box+BoxChild
		this.button1523 = new global::Gtk.Button ();
		this.button1523.CanFocus = true;
		this.button1523.Name = "button1523";
		this.button1523.UseUnderline = true;
		this.button1523.Label = global::Mono.Unix.Catalog.GetString ("Launch Parkitect");
		this.vbox7.Add (this.button1523);
		global::Gtk.Box.BoxChild w30 = ((global::Gtk.Box.BoxChild)(this.vbox7 [this.button1523]));
		w30.Position = 0;
		w30.Expand = false;
		w30.Fill = false;
		this.hbox3.Add (this.vbox7);
		global::Gtk.Box.BoxChild w31 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.vbox7]));
		w31.Position = 2;
		w31.Expand = false;
		w31.Fill = false;
		this.vbox3.Add (this.hbox3);
		global::Gtk.Box.BoxChild w32 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hbox3]));
		w32.PackType = ((global::Gtk.PackType)(1));
		w32.Position = 3;
		w32.Expand = false;
		w32.Fill = false;
		// Container child vbox3.Gtk.Box+BoxChild
		this.hseparator2 = new global::Gtk.HSeparator ();
		this.hseparator2.Name = "hseparator2";
		this.vbox3.Add (this.hseparator2);
		global::Gtk.Box.BoxChild w33 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hseparator2]));
		w33.PackType = ((global::Gtk.PackType)(1));
		w33.Position = 4;
		w33.Expand = false;
		w33.Fill = false;
		w33.Padding = ((uint)(1));
		this.Add (this.vbox3);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 633;
		this.DefaultHeight = 615;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.button56.Clicked += new global::System.EventHandler (this.Install_Mod);
		this.Mod_Website.Clicked += new global::System.EventHandler (this.Visit_Mod_Website);
		this.Mod_Update.Clicked += new global::System.EventHandler (this.Check_For_Update);
		this.Mod_Uninstall.Clicked += new global::System.EventHandler (this.Uninstall_Mod);
		this.button1500.Clicked += new global::System.EventHandler (this.Visit_Home_Website);
		this.button1499.Clicked += new global::System.EventHandler (this.Donate);
		this.button1523.Clicked += new global::System.EventHandler (this.Launch_Parkitect);
	}
}