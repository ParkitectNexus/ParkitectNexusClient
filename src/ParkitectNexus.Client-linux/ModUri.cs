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

        /// <summary>
        /// subit the URI and proceed with mod installation
        /// </summary>
        protected void Submit_URI (object sender, EventArgs e)
        {
            if (!ModDownload.Download (txtNexusURI.Text, _parkitect, _parkitectOnlineAssetRepository))
                this.Respond (Gtk.ResponseType.Cancel);
                

        }
    }
}

