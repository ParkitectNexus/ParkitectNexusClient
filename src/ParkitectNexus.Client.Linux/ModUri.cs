using System;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Presenter;
using Gtk;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client.Linux
{
    public partial class ModUri : Gtk.Dialog, IPresenter
    {
        private readonly IParkitect _parkitect;
        private readonly IParkitectOnlineAssetRepository _parkitectOnlineAssetRepository;
        private readonly IPresenterFactory _presenterFactory;
        private readonly ILogger _logger;
        public ModUri (IParkitect parkitect,ILogger logger,IParkitectOnlineAssetRepository parkitectOnlineAssetRepository,IPresenterFactory presenterFactory)
        {
            this.Build ();
            this._logger = logger;
            this._parkitectOnlineAssetRepository = parkitectOnlineAssetRepository;
            this._parkitect = parkitect;
            this._presenterFactory = presenterFactory;
        }

        /// <summary>
        /// subit the URI and proceed with mod installation
        /// </summary>
        protected void Submit_URI (object sender, EventArgs e)
        {
           
            // Try to parse the specified download url. If parsing fails open ParkitectNexus. 
            ParkitectNexusUrl parkitectNexusUrl;
            if (!ParkitectNexusUrl.TryParse (txtNexusURI.Text, out parkitectNexusUrl)) {
                // Create a form to allow the dialogs to have a owner with forced focus and an icon.
               MessageDialog errorDialog = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, "The URL you opened is invalid!");
                switch (errorDialog.Run ()) {
                    case(int) ResponseType.Ok:
                        break;
                default:
                    this.Destroy ();
                    break;
                }
                errorDialog.Destroy ();
            } else {

                // Run the download process in an installer form, for a nice visible process.

                var form = new ModInstallDialog(parkitectNexusUrl, this, _logger, _parkitect, _parkitectOnlineAssetRepository);
                switch (form.Run ()) {
                case (int)Gtk.ResponseType.Apply:
                    
                    form.Destroy ();
               break;
                default:
                    this.Respond (Gtk.ResponseType.Cancel);

                 break;
                }
                form.Destroy ();
                this.Destroy ();
            }
        

  
        }
    }
}

