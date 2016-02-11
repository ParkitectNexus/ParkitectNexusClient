// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using ParkitectNexus.Data.Assets.Modding;

namespace ParkitectNexus.Client.Windows.Controls.SliderPanels
{
    public partial class ModSliderPanel : SliderPanel
    {
        private readonly IModAsset _mod;

        public ModSliderPanel(IModAsset mod)
        {
            if (mod == null) throw new ArgumentNullException(nameof(mod));
            _mod = mod;

            InitializeComponent();

            nameLabel.Text = mod.Name;
            pictureBox.Image = mod.GetImage();
            versionLabel.Text = "???";
            enableModToggle.Checked = false;
        }

        private void enableModToggle_CheckedChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            //_mod.IsEnabled = enableModToggle.Checked;
        }
    }
}