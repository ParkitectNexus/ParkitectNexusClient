// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Tasks;
using ParkitectNexus.Data.Tasks.Prefab;

namespace ParkitectNexus.Client.Windows.Controls.SliderPanels
{
    public partial class ModSliderPanel : SliderPanel
    {
        private readonly IQueueableTaskManager _queueableTaskManager;
        private readonly IModAsset _mod;

        public ModSliderPanel(IQueueableTaskManager queueableTaskManager, IModAsset mod)
        {
            if (mod == null) throw new ArgumentNullException(nameof(mod));
            _queueableTaskManager = queueableTaskManager;
            _mod = mod;

            InitializeComponent();

            nameLabel.Text = mod.Name;
            pictureBox.Image = mod.GetImage();
            versionLabel.Text = _mod.Tag ?? "-";
            enableModToggle.Checked = _mod.Information.IsEnabled;

            debugTextBox.Text = mod.ToString();
        }

        private void enableModToggle_CheckedChanged(object sender, EventArgs e)
        {
            _mod.Information.IsEnabled = enableModToggle.Checked;
        }

        private void recompileButton_Click(object sender, EventArgs e)
        {
            _queueableTaskManager.Add(new CompileModTask(_mod));
        }
    }
}
