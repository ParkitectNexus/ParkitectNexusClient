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
		}

		public IParkitectMod GetParkitectMod{get{return _parkitectMod;}}

		[Gtk.TreeNodeValue (Column=2)]
		public string version
		{ 
			get
			{ 
				return _parkitectMod.Tag;
			}
		}

		[Gtk.TreeNodeValue (Column=1)]
		public string name
		{ 
			get
			{ 
				return _parkitectMod.Name;
			}
		}

		[Gtk.TreeNodeValue (Column=0)]
		public bool isActive{ 
			get{ 
				return _parkitectMod.IsEnabled;
			} 
			set
			{ 
				_parkitectMod.IsEnabled = value; 
				_parkitectMod.Save ();
			}
		}
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

		//toggle even for checkboxes in tree list
		CellRendererToggle toggle = new CellRendererToggle ();
		toggle.Mode = CellRendererMode.Activatable;
		toggle.Toggled += (o, args) => {
			((Mod)_mods.GetNode(new TreePath(args.Path))).isActive = !((Mod)_mods.GetNode(new TreePath(args.Path))).isActive;
		};

		//append the columns to the tree
		listViewMods.AppendColumn ("", toggle, "active", 0);
		listViewMods.AppendColumn ("Name", new CellRendererText (), "text", 1);
		listViewMods.AppendColumn ("Version", new CellRendererText (), "text", 2);

		listViewMods.ShowAll ();

		//update selected mod
		listViewMods.NodeSelection.Changed += (sender, e) => {
			Gtk.NodeSelection selection = (Gtk.NodeSelection) sender;
			Mod mod = (Mod) selection.SelectedNode;
			if(mod != null)
			{
				ShowMod(mod.GetParkitectMod);
				_selectedMod = mod;
			}
		};

		UpdateModList ();
		HideModInfo ();

		listViewMods.NodeStore = _mods;

	}
	/// <summary>
	/// Updates the mod list.
	/// </summary>
	private void UpdateModList()
	{
		_mods.Clear ();
		foreach (IParkitectMod mod in _parkitect.InstalledMods) {
			_mods.AddNode (new Mod (mod));
		}
	}

	/// <summary>
	/// Hides the mod info.
	/// </summary>
	private void HideModInfo()
	{
		lblModName.Text = "-";
		lblModVersion.Text = "-";
		lblDevelopmentStatus.Visible = false;

		btnCheckUpdate.Sensitive = false;
		btnModWebsite.Sensitive = false;
		btnUninstall.Sensitive = false;
	}

	/// <summary>
	/// display the specific mod
	/// </summary>
	private void ShowMod(IParkitectMod mod)
	{
		lblModName.Text = mod.Name;
		lblModVersion.Text = mod.Tag;
		lblDevelopmentStatus.Visible = mod.IsDevelopment;

		btnCheckUpdate.Sensitive = !mod.IsDevelopment && !string.IsNullOrWhiteSpace(mod.Repository);
		btnModWebsite.Sensitive = !mod.IsDevelopment && !string.IsNullOrWhiteSpace(mod.Repository);
		btnUninstall.Sensitive = !mod.IsDevelopment;
	}
		

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void CheckForUpdates (object sender, EventArgs e)
	{
		if (_selectedMod == null) return;

		try
		{
			var url = new ParkitectNexusUrl(_selectedMod.GetParkitectMod.Name, ParkitectAssetType.Mod, _selectedMod.GetParkitectMod.Repository);
			var info =  _parkitectOnlineAssetRepository.ResolveDownloadInfo(url).Result;


			if (info.Tag == _selectedMod.GetParkitectMod.Tag)
			{
				MessageDialog errorDialog = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, _selectedMod.GetParkitectMod.Name +" is already up to date!");
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

	protected void UninstallMod (object sender, EventArgs e)
	{
		if (_selectedMod == null) return;
		_mods.RemoveNode (_selectedMod);
		_selectedMod.GetParkitectMod.Delete();
		HideModInfo ();
		UpdateModList ();
	}

	protected void InstallMod (object sender, EventArgs e)
	{
		ModUri installMod = new ModUri (_parkitect,_parkitectOnlineAssetRepository);
		installMod.Run ();
		installMod.Destroy ();
		UpdateModList ();

	}
		

	protected void VisitModWebsite (object sender, EventArgs e)
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
