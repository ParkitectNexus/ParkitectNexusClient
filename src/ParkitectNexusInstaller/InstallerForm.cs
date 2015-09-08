// ParkitectNexusInstaller
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Linq;
using System.Windows.Forms;

namespace ParkitectNexusInstaller
{
    internal partial class InstallerForm : Form
    {
        private readonly Parkitect _parkitect;
        private readonly ParkitectNexus _parkitectNexus;
        private readonly ParkitectNexusUrl _parkitectNexusUrl;
        private int _dots;
        private int _dotsDir = 1;

        public InstallerForm(Parkitect parkitect, ParkitectNexus parkitectNexus, ParkitectNexusUrl parkitectNexusUrl)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (parkitectNexus == null) throw new ArgumentNullException(nameof(parkitectNexus));
            if (parkitectNexusUrl == null) throw new ArgumentNullException(nameof(parkitectNexusUrl));
            _parkitect = parkitect;
            _parkitectNexus = parkitectNexus;
            _parkitectNexusUrl = parkitectNexusUrl;
            InitializeComponent();

            installingLabel.Text = $"Installing {parkitectNexusUrl.AssetType} {parkitectNexusUrl.Name}";
        }

        private async void InstallAsset()
        {
            try
            {
                var asset = await _parkitectNexus.DownloadFile(_parkitectNexusUrl);

                if (asset == null)
                {
                    Close();
                    return;
                }

                await _parkitect.StoreAsset(asset);

                asset.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show(this,
                    $"Failed to install {_parkitectNexusUrl.AssetType.GetCustomAttribute<ParkitectAssetInfoAttribute>()?.Name}\nPlease try again later!",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            InstallAsset();
            base.OnLoad(e);
        }

        #endregion

        private void timer_Tick(object sender, EventArgs e)
        {
            _dots += _dotsDir;

            if (_dots <= 0 || _dots > 4)
                _dotsDir = -_dotsDir;

            statusLabel.Text = "Downloading." + string.Concat(Enumerable.Repeat(".", _dots));
        }
    }
}