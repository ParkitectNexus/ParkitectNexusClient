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
using System.Threading;

public partial class MainWindow: Gtk.Window
{
		
	private readonly IParkitect _parkitect;
	private readonly IParkitectOnlineAssetRepository _parkitectOnlineAssetRepository;
	private readonly IParkitectNexusWebsite _parkitectNexusWebsite;
	private NodeStore _mods = new NodeStore(typeof(TreeNodeModContainer));

	private TreeNodeModContainer _selectedMod;

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
	

		//toggle for checkboxes in tree list
		CellRendererToggle toggle = new CellRendererToggle ();
		toggle.Mode = CellRendererMode.Activatable;
		toggle.Toggled += (o, args) => {
			((TreeNodeModContainer)_mods.GetNode(new TreePath(args.Path))).IsActive = !((TreeNodeModContainer)_mods.GetNode(new TreePath(args.Path))).IsActive;
		};


		CellRendererText nameRenderText = new CellRendererText ();

		//append the columns to the tree
		listViewMods.AppendColumn ("", toggle, "active", 0);
		listViewMods.AppendColumn ("Name", nameRenderText, "text", 1);
		listViewMods.AppendColumn ("Current",new CellRendererText () , "text", 2);
		listViewMods.AppendColumn ("New", new CellRendererText (), "text", 3);

		//style the name column for the case when the current tag and avalible tag don't match
		listViewMods.Columns [1].SetCellDataFunc (nameRenderText, new TreeCellDataFunc (nameRendering));

		listViewMods.ShowAll ();

		//update selected mod
		listViewMods.NodeSelection.Changed += (sender, e) => {
			Gtk.NodeSelection selection = (Gtk.NodeSelection) sender;
			TreeNodeModContainer mod = (TreeNodeModContainer) selection.SelectedNode;
			if(mod != null)
			{
				ShowMod(mod.GetParkitectMod);
				_selectedMod = mod;
			}
		};
			
		listViewMods.NodeStore = _mods;

		//update the tag information
		listViewMods.Model.RowInserted += async (o, args) => {
			try{
				var mod = (TreeNodeModContainer)((NodeStore)o).GetNode(args.Path);
				if (mod.GetParkitectMod.Repository != null && mod.GetParkitectMod.Name != null) {
					var url = new ParkitectNexusUrl (mod.GetParkitectMod.Name, ParkitectAssetType.Mod, mod.GetParkitectMod.Repository);
					var info = await _parkitectOnlineAssetRepository.ResolveDownloadInfo (url);	
					mod.AvaliableVersion = info.Tag;
					listViewMods.QueueDraw();	
				}
			}
			catch (Exception)
			{
				
			}
		};

		UpdateModList ();
		HideModInfo ();
	}

	/// <summary>
	/// update tag styling
	/// </summary>
	private void nameRendering (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
	{
		string currentVersion = (string)model.GetValue (iter, 2);
		string avaliableVersion = (string)model.GetValue (iter, 3);

		if (currentVersion!= avaliableVersion && avaliableVersion != "-") {
			(cell as Gtk.CellRendererText).Foreground = "Red";
		} else {
			(cell as Gtk.CellRendererText).Foreground = "Black";
		}
	}


	/// <summary>
	/// Updates the mod list.
	/// </summary>
	private void UpdateModList()
	{
		_mods.Clear ();
		foreach (IParkitectMod mod in _parkitect.InstalledMods) {
			_mods.AddNode (new TreeNodeModContainer (mod));
		}
	}

	/// <summary>
	/// Hides the mod info.
	/// </summary>
	private void HideModInfo()
	{
		lblModName.Text = "-";
		lblModVersion.Text = "-";
		lblViewOnParkitectNexusWebsite.Text = "-";
		lblViewOnParkitectNexusWebsite.ModifyFg (StateType.Normal, new Gdk.Color (0, 0, 0));

		lblDevelopmentStatus.Visible = false;
		btnCheckUpdate.Sensitive = false;
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

		lblViewOnParkitectNexusWebsite.ModifyFg (StateType.Normal, new Gdk.Color (0, 0, 255));
		lblViewOnParkitectNexusWebsite.Text = "View on ParkitectNexus";
		lblViewOnParkitectNexusWebsite.UseUnderline = true;
		lblViewOnParkitectNexusWebsite.Pattern = "______________________";

		btnCheckUpdate.Sensitive = !mod.IsDevelopment && !string.IsNullOrWhiteSpace(mod.Repository);
		btnUninstall.Sensitive = !mod.IsDevelopment;
	}
		
	/// <summary>
	/// Raises the delete event event.
	/// </summary>
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	/// <summary>
	/// Check for updates and update the associated mod
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="e">E.</param>
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

				//update the tag info and update the mod info
				_selectedMod.AvaliableVersion = info.Tag;
				_selectedMod.GetParkitectMod.Tag = info.Tag;
				ShowMod(_selectedMod.GetParkitectMod);

				listViewMods.QueueDraw();
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
		
	/// <summary>
	/// Launche Parkitect.
	/// </summary>
	protected void LaunchParkitect (object sender, EventArgs e)
	{
		this.Hide ();
		_parkitect.Launch ();
	}

	/// <summary>
	/// launch a brower with the home site
	/// </summary>
	protected void Visit_Home_Website (object sender, EventArgs e)
	{
		_parkitectNexusWebsite.Launch();
	}
		

	/// <summary>
	/// open donate dialog
	/// </summary>
	protected void Donate (object sender, EventArgs e)
	{
		Gtk.MessageDialog donateInfo = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.YesNo, "Maintaining this client and adding new features takes a lot of time.\n" +
			"If you appreciate our work, please consider sending a donation our way!\n" +
			"All donations will be used for further development of the ParkitectNexus Client and the website.\n" +
			"\nSelect Yes to visit PayPal and send a donation.");
		if(donateInfo.Run() == (int)ResponseType.Yes)
			Process.Start("https://paypal.me/ikkentim");
		donateInfo.Destroy();


	}
		

	/// <summary>
	/// Visit the associated mod on the website
	/// </summary>
	protected void VisitModWebsite (object o, ButtonPressEventArgs args)
	{
		if (_selectedMod == null) return;
		Process.Start($"https://client.parkitectnexus.com/redirect/{_selectedMod.GetParkitectMod.Repository}");
	
	}

}
