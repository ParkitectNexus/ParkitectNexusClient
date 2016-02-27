// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Diagnostics;
using System.Windows.Forms;
using MetroFramework;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Tasks;
using ParkitectNexus.Data.Tasks.Prefab;
using ParkitectNexus.Data.Web;

namespace ParkitectNexus.Client.Windows.Controls.SliderPanels
{
    public partial class ModSliderPanel : SliderPanel
    {
        private readonly IQueueableTaskManager _queueableTaskManager;
        private readonly IParkitect _parkitect;
        private readonly IWebsite _website;
        private readonly IModAsset _mod;

        public ModSliderPanel(IQueueableTaskManager queueableTaskManager, IParkitect parkitect, IWebsite website, IModAsset mod)
        {
            if (mod == null) throw new ArgumentNullException(nameof(mod));
            _queueableTaskManager = queueableTaskManager;
            _parkitect = parkitect;
            _website = website;
            _mod = mod;

            InitializeComponent();

            nameLabel.Text = mod.Name;
            pictureBox.Image = mod.GetImage();
            versionLabel.Text = _mod.Tag ?? "-";
            enableModToggle.Checked = _mod.Information.IsEnabled;

            debugTextBox.Text = mod.ToString();
            metroButton1.Enabled = updateButton.Enabled = deleteButton.Enabled =
                !mod.Information.IsDevelopment && !string.IsNullOrWhiteSpace(mod.Repository) &&
                !string.IsNullOrWhiteSpace(mod.Tag);
        }

        private void enableModToggle_CheckedChanged(object sender, EventArgs e)
        {
            _mod.Information.IsEnabled = enableModToggle.Checked;
        }

        private void recompileButton_Click(object sender, EventArgs e)
        {
            _queueableTaskManager.With(_mod).Add<CompileModTask>();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (
                MetroMessageBox.Show(this, $"Are you sure you wish to delete '{_mod.Name}'?", $"Delete mod",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            _parkitect.Assets.DeleteAsset(_mod);
            var mainForm = ParentForm as MainForm;
            mainForm?.SpawnSliderPanel(null);
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            _queueableTaskManager.With(_mod).Add<UpdateModTask>();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Process.Start(_website.ResolveUrl($"redirect/{_mod.Repository}", "client"));
        }
    }
}
