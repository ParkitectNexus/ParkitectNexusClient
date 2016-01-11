﻿// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Linq;
using System.Windows.Forms;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;

namespace ParkitectNexus.Client.Wizard
{
    internal partial class InstallAssetUserControl : BaseWizardUserControl
    {
        private readonly IParkitect _parkitect;
        private readonly IParkitectNexusUrl _parkitectNexusUrl;
        private readonly IParkitectOnlineAssetRepository _parkitectOnlineAssetRepository;
        private readonly ILogger _logger;
        private readonly BaseWizardUserControl _returnControl;
        private int _dots;
        private int _dotsDirection = 1;
        private string _keyword = "Downloading";

        public InstallAssetUserControl(IParkitect parkitect,
            IParkitectOnlineAssetRepository parkitectOnlineAssetRepository, ILogger logger, IParkitectNexusUrl parkitectNexusUrl,
            BaseWizardUserControl returnControl)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (parkitectOnlineAssetRepository == null)
                throw new ArgumentNullException(nameof(parkitectOnlineAssetRepository));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (parkitectNexusUrl == null) throw new ArgumentNullException(nameof(parkitectNexusUrl));
            _parkitect = parkitect;
            _parkitectOnlineAssetRepository = parkitectOnlineAssetRepository;
            _logger = logger;
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
                _logger.WriteLine($"Failed to install {assetName}!");
                _logger.WriteException(e);

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
            closeTimer.Enabled = false;
            downloadingTimer.Enabled = false;

            if (_returnControl == null)
                WizardForm.Close();
            else
                WizardForm.Attach(_returnControl);
        }
    }
}
