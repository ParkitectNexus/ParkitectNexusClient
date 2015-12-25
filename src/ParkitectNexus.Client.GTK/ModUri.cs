using System;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;

namespace ParkitectNexus.Client.GTK
{
	public partial class ModUri : Gtk.Dialog
	{
		private readonly IParkitect _parkitect;
		private readonly IParkitectOnlineAssetRepository _parkitectOnlineAssetRepository;

		public ModUri (IParkitect parkitect,IParkitectOnlineAssetRepository parkitectOnlineAssetRepository)
		{
			this.Build ();
			this._parkitectOnlineAssetRepository = parkitectOnlineAssetRepository;
			this._parkitect = parkitect;
		}

		protected void Submit_URI (object sender, EventArgs e)
		{
			if (!ModDownload.Download (Nexus_URI.Text, _parkitect, _parkitectOnlineAssetRepository))
				this.Respond (Gtk.ResponseType.Cancel);
				

		}
	}
}

