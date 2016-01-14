using System;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Game;
using System.Threading;
using System.Linq;
using Gtk;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Presenter;

namespace ParkitectNexus.Client.Linux
{
    public partial class ModInstallDialog : Gtk.Dialog
    {
        public ParkitectNexusUrl _ParkitectNexusUrl;
        private IParkitect _parkitect;
        private IParkitectOnlineAssetRepository _assetRepository;
        private int _dots;
        private int _dotsDirection = 1;
        private string _keyword = "Downloading";

        private ILogger _logger;

        private volatile bool isFinished = false;

        public ModInstallDialog (ParkitectNexusUrl parkitectNexusUrl,IPresenter presenter,ILogger logger,IParkitect parkitect, IParkitectOnlineAssetRepository parkitectOnlineAssetRepository)
        {

            this.Parent = (Dialog)presenter;

            
            this._ParkitectNexusUrl = parkitectNexusUrl;
            this._logger = logger;
            this._parkitect = parkitect;
            this._assetRepository = parkitectOnlineAssetRepository;
            this.Build ();

            if (_parkitect == null) throw new ArgumentNullException(nameof(_parkitect));
            if (_assetRepository == null)
                throw new ArgumentNullException(nameof(_ParkitectNexusUrl));
            if (_ParkitectNexusUrl == null) throw new ArgumentNullException(nameof(_ParkitectNexusUrl));

            // Format the "installing" label.
            //installingLabel.Text = "Please wait while ParkitectNexus is installing {parkitectNexusUrl.AssetType} \"{parkitectNexusUrl.Name}\".";
            this.lblModName.Text = "Please wait while ParkitectNexus is installing " + _ParkitectNexusUrl.AssetType + " \"" + _ParkitectNexusUrl.Name + "\".";

            GLib.Timeout.Add(100, new GLib.TimeoutHandler(UpdateProgress));
            GLib.Timeout.Add(100, new GLib.TimeoutHandler(DownloadLabelUpdate));


            Thread download = new Thread(new ThreadStart(Process));
            download.Start();

        }

        /// <summary>
        /// update the download label
        /// </summary>
        private bool DownloadLabelUpdate()
        {
            if (isFinished == true) {
                lblProgressLabel.Text = "Done!";
                return false;
            } else {
                //Downloading... Effect. 

                // Add or subtract dots.
                _dots += _dotsDirection;

                // If the dots count is out of the boundaries, flip the dots direction.
                if (_dots <= 0 || _dots > 4)
                    _dotsDirection = -_dotsDirection;

                // Update the status label.
                lblProgressLabel.Text = _keyword + "." + string.Concat (Enumerable.Repeat (".", _dots));
            }
            return true;
        }

        /// <summary>
        /// update the loading bar
        /// </summary>
        private bool UpdateProgress()
        {
            if (isFinished == true) {
                if (!btnFinished.Sensitive) {
                    installProgress.Activate ();
                    btnFinished.Sensitive = true;
                }
                installProgress.Fraction += 0.1;
                if (installProgress.Fraction >= 1)
                    return false;
            } else {
                installProgress.Pulse ();

            }
            return true;
        }

        /// <summary>
        /// Process and install the mods
        /// </summary>
        private async void Process()
        {

            var assetName = _ParkitectNexusUrl.AssetType.GetCustomAttribute<ParkitectAssetInfoAttribute>()?.Name;
            try
            {
                // Download the asset.
                var asset = await _assetRepository.DownloadFile(_ParkitectNexusUrl);

                if (asset == null)
                {
                    Gtk.Application.Invoke (delegate {
                        Gtk.MessageDialog errorDialog = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.YesNo, $"Failed to install {assetName}!\nPlease try again later.","ParkitectNexus Client");
                        errorDialog.Run (); 
                        this.Destroy();
                        this.Respond(ResponseType.Close);

                    });
                    return;
                }

                _keyword = "Installing";

                await _parkitect.StoreAsset(asset);

                asset.Dispose();
            }
            catch (Exception e)
            {
                _logger.WriteLine($"Failed to install {assetName}!");
                _logger.WriteException(e);

                // If the asset has failed to download, show some feedback to the user.
                Gtk.Application.Invoke (delegate {
                    Gtk.MessageDialog errorDialog = new MessageDialog (this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.YesNo, $"Failed to install {assetName}!\nPlease try again later.\n\n{e.Message}");
                    errorDialog.Run (); 
                    this.Destroy();
                    this.Respond(ResponseType.Close);

                });

            }
            finally
            {
                isFinished = true;

            }

        }

    }
}

