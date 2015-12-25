using System;
using Gtk;
using System.Collections;
using System.Collections.Generic;
using ParkitectNexus.Mod.ModLoader;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;
using System.Linq;
using System.Diagnostics;
using ParkitectNexus.Client.GTK;

public partial class MainWindow: Gtk.Window
{
	[Gtk.TreeNode (ListOnly=true)]
	public class Mod : Gtk.TreeNode
	{
		private IParkitectMod _parkitectMod;
		public Mod(IParkitectMod parkitectMod)
		{
			this._parkitectMod = parkitectMod;
			this.name = parkitectMod.Name;
			this.isActive = parkitectMod.IsEnabled;
		}

		public IParkitectMod GetParkitectMod{get{return _parkitectMod;}}

		[Gtk.TreeNodeValue (Column=1)]
		public string name{ get; set;}

		[Gtk.TreeNodeValue (Column=0)]
		public bool isActive{ get; set;}
	}
		
	private readonly IParkitect _parkitect;
	private readonly IParkitectOnlineAssetRepository _parkitectOnlineAssetRepository;
	private readonly IParkitectNexusWebsite _parkitectNexusWebsite;
	private NodeStore _mods = new NodeStore(typeof(Mod));

	private Mod _selectedMod;

	public MainWindow (IParkitectNexusWebsite parkitectNexusWebsite,IParkitect parkitect,IParkitectOnlineAssetRepository parkitectOnlineAssetRepository) :
	base (Gtk.WindowType.Toplevel)
	{
		if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
		if (parkitectOnlineAssetRepository == null)
			throw new ArgumentNullException(nameof(parkitectOnlineAssetRepository));
		
		_parkitect = parkitect;
		_parkitectOnlineAssetRepository = parkitectOnlineAssetRepository;
		_parkitectNexusWebsite = parkitectNexusWebsite;

		Build ();
		CellRendererToggle toggle = new CellRendererToggle ();
		toggle.Mode = CellRendererMode.Activatable;
		toggle.Toggled += (o, args) => {
			((Mod)_mods.GetNode(new TreePath(args.Path))).isActive = !((Mod)_mods.GetNode(new TreePath(args.Path))).isActive;
		};
		Mods.AppendColumn ("", toggle, "active", 0);
		Mods.AppendColumn ("", new CellRendererText (), "text", 1);

		Mods.ShowAll ();

		Mods.NodeSelection.Changed += (sender, e) => {
			Gtk.NodeSelection selection = (Gtk.NodeSelection) sender;
			Mod mod = (Mod) selection.SelectedNode;
			if(mod != null)
			{
				ShowMod(mod.GetParkitectMod);
				_selectedMod = mod;
			}
		};


		foreach (IParkitectMod mod in _parkitect.InstalledMods) {
			_mods.AddNode (new Mod (mod));
		}
		HideModInfo ();

		Mods.NodeStore = _mods;

	}


	private void HideModInfo()
	{
		Mod_Name.Text = "-";
		Mod_Version.Text = "-";
		Mov_Development_Status.Visible = false;

		Mod_Update.Sensitive = false;
		Mod_Website.Sensitive = false;
		Mod_Uninstall.Sensitive = false;
	}

	private void ShowMod(IParkitectMod mod)
	{
		Mod_Name.Text = mod.Name;
		Mod_Version.Text = mod.Tag;
		Mov_Development_Status.Visible = mod.IsDevelopment;

		Mod_Update.Sensitive = !mod.IsDevelopment && !string.IsNullOrWhiteSpace(mod.Repository);
		Mod_Website.Sensitive = !mod.IsDevelopment && !string.IsNullOrWhiteSpace(mod.Repository);
		Mod_Uninstall.Sensitive = !mod.IsDevelopment;
	}


	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void Start (object sender, EventArgs e)
	{
		var checkbox = new global::Gtk.Button ();
		checkbox.Label = global::Mono.Unix.Catalog.GetString ("GtkButton");


	}

	protected void Check_For_Update (object sender, EventArgs e)
	{
		if (_selectedMod == null) return;

		try
		{
			var url = new ParkitectNexusUrl(_selectedMod.GetParkitectMod.Name, ParkitectAssetType.Mod, _selectedMod.GetParkitectMod.Repository);
			var info =  _parkitectOnlineAssetRepository.ResolveDownloadInfo(url).Result;


			if (info.Tag == _selectedMod.GetParkitectMod.Tag)
			{
				Gtk.MessageDialog errorDialog = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.YesNo, "{_selectedMod.GetParkitectMod} is already up to date!");
				errorDialog.Run();
				errorDialog.Destroy();

			}
			else
			{
				ModDownload.Download(url,_parkitect,_parkitectOnlineAssetRepository);

			}
		}
		catch (Exception)
		{
			Gtk.MessageDialog errorDialog = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.YesNo, "Failed to check for updates. Please try again later.");
			errorDialog.Run();
			errorDialog.Destroy();
		}
	}

	protected void Uninstall_Mod (object sender, EventArgs e)
	{
		if (_selectedMod == null) return;
		_mods.RemoveNode (_selectedMod);
		_selectedMod.GetParkitectMod.Delete();
	}

	protected void Add_Mod (object sender, EventArgs e)
	{
		throw new NotImplementedException ();
	}
		

	protected void Visit_Mod_Website (object sender, EventArgs e)
	{
		if (_selectedMod == null) return;
		Process.Start($"https://client.parkitectnexus.com/redirect/{_selectedMod.GetParkitectMod.Repository}");
	}

	protected void Launch_Parkitect (object sender, EventArgs e)
	{
		this.Hide ();
		_parkitect.Launch ();

	}

	protected void Visit_Home_Website (object sender, EventArgs e)
	{
		_parkitectNexusWebsite.Launch();
	}

	protected void Donate (object sender, EventArgs e)
	{
		Gtk.MessageDialog donateInfo = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.YesNo, "Maintaining this client and adding new features takes a lot of time.\n" +
			"If you appreciate our work, please consider sending a donation our way!\n" +
			"All donations will be used for further development of the ParkitectNexus Client and the website.\n" +
			"\nSelect Yes to visit PayPal and send a donation.");
		donateInfo.Run();
		Process.Start("https://paypal.me/ikkentim");
		donateInfo.Destroy();


	}

}
