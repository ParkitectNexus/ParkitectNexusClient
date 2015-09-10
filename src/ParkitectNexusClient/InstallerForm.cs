// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ParkitectNexusClient
{
    /// <summary>
    ///     Represents an installation form.
    /// </summary>
    internal partial class InstallerForm : Form
    {
        private readonly Parkitect _parkitect;
        private readonly ParkitectNexus _parkitectNexus;
        private readonly ParkitectNexusUrl _parkitectNexusUrl;
        private int _dots;
        private int _dotsDirection = 1;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InstallerForm" /> class.
        /// </summary>
        /// <param name="parkitect">The parkitect.</param>
        /// <param name="parkitectNexus">The parkitect nexus.</param>
        /// <param name="parkitectNexusUrl">The parkitect nexus URL.</param>
        /// <exception cref="ArgumentNullException">Thrown if parkitect, parkitectNexus or parkitectNexusUrl is null.</exception>
        public InstallerForm(Parkitect parkitect, ParkitectNexus parkitectNexus, ParkitectNexusUrl parkitectNexusUrl)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (parkitectNexus == null) throw new ArgumentNullException(nameof(parkitectNexus));
            if (parkitectNexusUrl == null) throw new ArgumentNullException(nameof(parkitectNexusUrl));
            _parkitect = parkitect;
            _parkitectNexus = parkitectNexus;
            _parkitectNexusUrl = parkitectNexusUrl;
            InitializeComponent();

            // Format the "installing" label.
            installingLabel.Text = $"Installing {parkitectNexusUrl.AssetType} {parkitectNexusUrl.Name}";

            // Set the client size to make the baner fit snugly.
            ClientSize = new Size(493, 125);
        }

        /// <summary>
        ///     Installs the asset.
        /// </summary>
        private async void InstallAsset()
        {
            var assetName = _parkitectNexusUrl.AssetType.GetCustomAttribute<ParkitectAssetInfoAttribute>()?.Name;
            try
            {
                // Download the asset.
                var asset = await _parkitectNexus.DownloadFile(_parkitectNexusUrl);


                if (asset == null)
                {
                    // If the asset has failed to download, show some feedback to the user.
                    MessageBox.Show(this,
                        $"Failed to install {assetName}\nPlease try again later!",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }

                await _parkitect.StoreAsset(asset);

                asset.Dispose();
            }
            catch (Exception)
            {
                // If the asset has failed to download, show some feedback to the user.
                MessageBox.Show(this,
                    $"Failed to install {assetName}\nPlease try again later!",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // todo: Maybe these errors should be sent to some server.
            }
            finally
            {
                Close();
            }
        }

        #region Overrides of Form

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
        protected override void OnLoad(EventArgs e)
        {
            // Once the form is loading, start the downloading task.
            InstallAsset();
            base.OnLoad(e);
        }

        #endregion

        private void timer_Tick(object sender, EventArgs e)
        {
            //Downloading... Effect. 

            // Add or subtract dots.
            _dots += _dotsDirection;

            // If the dots count is out of the boundaries, flip the dots direction.
            if (_dots <= 0 || _dots > 4)
                _dotsDirection = -_dotsDirection;

            // Update the status label.
            statusLabel.Text = "Downloading." + string.Concat(Enumerable.Repeat(".", _dots));
        }
    }
}