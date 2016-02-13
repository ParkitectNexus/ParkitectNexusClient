using System;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Presenter;
using Gtk;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Tasks;
using ParkitectNexus.Data.Tasks.Prefab;
using ParkitectNexus.Data.Web.Models;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Web.API;

namespace ParkitectNexus.Client.Linux
{
    public partial class ModUri : Gtk.Dialog, IPresenter
    {
        private readonly IParkitect _parkitect;
       private readonly IPresenterFactory _presenterFactory;
        private readonly ILogger _logger;
		private IQueueableTaskManager _queuableTaskManager;
		private IRemoteAssetRepository _assetRepository;
		private IParkitectNexusAPI _nexusAPI;
        private IWebsite _website;
        public ModUri (IWebsite website,IQueueableTaskManager queuableTaskManager, IRemoteAssetRepository assetRepositry,IParkitectNexusAPI nexusAPI,IQueueableTaskManager taskManager,IParkitect parkitect,ILogger logger,IPresenterFactory presenterFactory)
        {
            
			this.Build ();
            this._website = website;
			this._assetRepository = assetRepositry;
			this._nexusAPI = nexusAPI;

			this._logger = logger;
            this._parkitect = parkitect;
            this._presenterFactory = presenterFactory;
			this._queuableTaskManager = queuableTaskManager;
		}

        /// <summary>
        /// subit the URI and proceed with mod installation
        /// </summary>
        protected void Submit_URI (object sender, EventArgs e)
        {
           
            // Try to parse the specified download url. If parsing fails open ParkitectNexus. 
            NexusUrl parkitectNexusUrl;
            if (!NexusUrl.TryParse (txtNexusURI.Text, out parkitectNexusUrl)) {
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

				var assetTask =new InstallAssetTask (_parkitect, _website, _assetRepository);
				assetTask.Data = parkitectNexusUrl.Data;
				// Run the download process in an installer form, for a nice visible process.


                NexusUrl nexusURL;
                NexusUrl.TryParse (txtNexusURI.Text, out nexusURL);
					var task = new InstallAssetTask (_parkitect, _website, _assetRepository);
					task.Data = nexusURL.Data;
					_queuableTaskManager.Add (task);


				/*switch (form.Run ()) {
                case (int)Gtk.ResponseType.Apply:
                    
                    form.Destroy ();
               break;
                default:
                    this.Respond (Gtk.ResponseType.Cancel);

                 break;
                }
                form.Destroy ();*/
                this.Destroy ();
            }
        

  
        }
    }
}

