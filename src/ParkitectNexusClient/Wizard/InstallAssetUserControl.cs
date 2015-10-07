// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Linq;
using System.Windows.Forms;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client.Wizard
{
    internal partial class InstallAssetUserControl : BaseWizardUserControl
    {
        private readonly Parkitect _parkitect;
        private readonly ParkitectOnlineAssetRepository _parkitectOnlineAssetRepository;
        private readonly ParkitectNexusUrl _parkitectNexusUrl;
        private readonly BaseWizardUserControl _returnControl;
        private int _dots;
        private int _dotsDirection = 1;
        private string _keyword = "Downloading";

        public InstallAssetUserControl(Parkitect parkitect,
            ParkitectOnlineAssetRepository parkitectOnlineAssetRepository, ParkitectNexusUrl parkitectNexusUrl,
            BaseWizardUserControl returnControl)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (parkitectOnlineAssetRepository == null)
                throw new ArgumentNullException(nameof(parkitectOnlineAssetRepository));
            if (parkitectNexusUrl == null) throw new ArgumentNullException(nameof(parkitectNexusUrl));
            _parkitect = parkitect;
            _parkitectOnlineAssetRepository = parkitectOnlineAssetRepository;
            _parkitectNexusUrl = parkitectNexusUrl;
            _returnControl = returnControl;

            InitializeComponent();

            // Format the "installing" label.
            installingLabel.Text =
                $"Please wait while ParkitectNexus is installing {parkitectNexusUrl.AssetType} \"{parkitectNexusUrl.Name}\".";
        }

        private async void InstallAsset()
        {
            var assetName = _parkitectNexusUrl.AssetType.GetCustomAttribute<ParkitectAssetInfoAttribute>()?.Name;
            try
            {
                // Download the asset.
                var asset = await _parkitectOnlineAssetRepository.DownloadFile(_parkitectNexusUrl);

                if (asset == null)
                {
                    // If the asset has failed to download, show some feedback to the user.
                    MessageBox.Show(this,
                        $"Failed to install {assetName}!\nPlease try again later.",
                        "ParkitectNexus Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    WizardForm.Close();
                    return;
                }

                _keyword = "Installing";

                await _parkitect.StoreAsset(asset);
                
                asset.Dispose();
            }
            catch (Exception e)
            {
                // If the asset has failed to download, show some feedback to the user.
                MessageBox.Show(this,
                    $"Failed to install {assetName}!\nPlease try again later.\n\n{e.Message}",
                    "ParkitectNexus Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                downloadingTimer.Enabled = false;
                statusLabel.Text = "Done!";
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = 100;
                WizardForm.Cursor = DefaultCursor;
                finishButton.Enabled = true;
                closeTimer.Enabled = true;
            }
        }

        #region Overrides of BaseWizardUserControl

        protected override void OnAttached()
        {
            InstallAsset();
            WizardForm.Cursor = Cursors.WaitCursor;
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
            statusLabel.Text = _keyword + "." + string.Concat(Enumerable.Repeat(".", _dots));
        }

        private void closeTimer_Tick(object sender, EventArgs e)
        {
            if (_returnControl == null)
                WizardForm.Close();
            else WizardForm.Attach(_returnControl);
            closeTimer.Enabled = false;
            downloadingTimer.Enabled = false;
        }

        private void finishButton_Click(object sender, EventArgs e)
        {
            if (_returnControl == null)
                WizardForm.Close();
            else WizardForm.Attach(_returnControl);
        }
    }
}