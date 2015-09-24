using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParkitectNexus.Data;

namespace ParkitectNexus.Client.Wizard
{
    internal partial class InstallAssetUserControl : BaseWizardUserControl
    {
        private readonly Parkitect _parkitect;
        private readonly ParkitectNexusWebsite _parkitectNexus;
        private readonly ParkitectNexusUrl _parkitectNexusUrl;
        private int _dots;
        private int _dotsDirection = 1;

        public InstallAssetUserControl(Parkitect parkitect, ParkitectNexusWebsite parkitectNexus, ParkitectNexusUrl parkitectNexusUrl)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (parkitectNexus == null) throw new ArgumentNullException(nameof(parkitectNexus));
            if (parkitectNexusUrl == null) throw new ArgumentNullException(nameof(parkitectNexusUrl));
            _parkitect = parkitect;
            _parkitectNexus = parkitectNexus;
            _parkitectNexusUrl = parkitectNexusUrl;

            InitializeComponent();

            // Format the "installing" label.
            installingLabel.Text = $"Please wait while ParkitectNexus is installing {parkitectNexusUrl.AssetType} \"{parkitectNexusUrl.Name}\".";
        }

        public InstallAssetUserControl()
        {
            InitializeComponent();
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
                        $"Failed to install {assetName}!\nPlease try again later.",
                        "ParkitectNexus Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    WizardForm.Close();
                    return;
                }

                await _parkitect.StoreAsset(asset);

                asset.Dispose();
            }
            catch (Exception)
            {
                // If the asset has failed to download, show some feedback to the user.
                MessageBox.Show(this,
                    $"Failed to install {assetName}!\nPlease try again later.",
                    "ParkitectNexus Client", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // todo: Maybe these errors should be sent to some server.
            }
            finally
            {
                downloadingTimer.Enabled = false;
                statusLabel.Text = "Done!";
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 100;
                Cursor = DefaultCursor;
                finishButton.Enabled = true;
                closeTimer.Enabled = true;
            }
        }
        
        #region Overrides of BaseWizardUserControl

        protected override void OnAttached()
        {
            InstallAsset();
            base.OnAttached();
        }

        #endregion

        private void downloadingTimer_Tick(object sender, EventArgs e)
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

        private void closeTimer_Tick(object sender, EventArgs e)
        {
            WizardForm.Close();
        }
        
        private void finishButton_Click(object sender, EventArgs e)
        {
            WizardForm.Close();
        }
    }
}
