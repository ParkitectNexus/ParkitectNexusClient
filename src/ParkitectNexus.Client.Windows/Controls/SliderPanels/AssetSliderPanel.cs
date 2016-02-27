// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Windows.Forms;
using MetroFramework;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Client.Windows.Controls.SliderPanels
{
    public partial class AssetSliderPanel : SliderPanel
    {
        private readonly IAsset _asset;
        private readonly IParkitect _parkitect;

        public AssetSliderPanel(IAsset asset, IParkitect parkitect)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));

            _asset = asset;
            _parkitect = parkitect;

            InitializeComponent();
            nameLabel.Text = asset.Name;
            pictureBox.Image = asset.GetImage();

            BackText = asset.Type.ToString();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (
                MetroMessageBox.Show(this, $"Are you sure you wish to delete '{_asset.Name}'?", $"Delete {_asset.Type.ToString().ToLower()}",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            _parkitect.Assets.DeleteAsset(_asset);
            var mainForm = ParentForm as MainForm;
            mainForm?.SpawnSliderPanel(null);
        }
    }
}
