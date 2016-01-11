// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;

namespace ParkitectNexus.Client.Wizard
{
    internal partial class ManageModsUserControl : BaseWizardUserControl
    {
        private readonly ILogger _logger;
        private readonly MenuUserControl _menu;
        private readonly IParkitect _parkitect;
        private readonly IParkitectOnlineAssetRepository _parkitectOnlineAssetRepository;

        private bool _disableChecking = true;

        public ManageModsUserControl(MenuUserControl menu, IParkitect parkitect,
            IParkitectOnlineAssetRepository parkitectOnlineAssetRepository, ILogger logger)
        {
            if (menu == null) throw new ArgumentNullException(nameof(menu));
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (parkitectOnlineAssetRepository == null)
                throw new ArgumentNullException(nameof(parkitectOnlineAssetRepository));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _menu = menu;
            _parkitect = parkitect;
            _parkitectOnlineAssetRepository = parkitectOnlineAssetRepository;
            _logger = logger;

            InitializeComponent();
        }

        private IParkitectMod SelectedMod => modsCheckedListBox.SelectedItem as IParkitectMod;

        #region Overrides of BaseWizardUserControl

        protected override void OnAttached()
        {
            FillListBox();
            base.OnAttached();
        }

        #endregion

        private void FillListBox()
        {
            modsCheckedListBox.Items.Clear();
            modsCheckedListBox.Items.AddRange(_parkitect.InstalledMods.ToArray());

            _disableChecking = false;
            for (var i = 0; i < modsCheckedListBox.Items.Count; i++)
            {
                var mod = ((IParkitectMod) modsCheckedListBox.Items[i]);
                modsCheckedListBox.SetItemChecked(i, mod.IsEnabled || mod.IsDevelopment);
            }
            _disableChecking = true;

            HideMod();
        }

        private void HideMod()
        {
            optionsGroupBox.Enabled = false;
            modNameLabel.Text = "-";
            modVersionLabel.Text = "-";
            enableModCheckBox.Checked = false;
            modInDevelopmentLabel.Visible = false;
        }

        private void ShowMod(IParkitectMod mod)
        {
            optionsGroupBox.Enabled = true;
            modNameLabel.Text = mod.Name;
            modVersionLabel.Text = mod.Tag;
            enableModCheckBox.Checked = mod.IsEnabled || mod.IsDevelopment;
            modInDevelopmentLabel.Visible = mod.IsDevelopment;
            enableModCheckBox.Enabled = !mod.IsDevelopment;
            updateButton.Enabled = !mod.IsDevelopment && !string.IsNullOrWhiteSpace(mod.Repository);
            parkitectNexusLinkLabel.Enabled = !mod.IsDevelopment && !string.IsNullOrWhiteSpace(mod.Repository);
            uninstallButton.Enabled = !mod.IsDevelopment;
        }

        private void modsCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedMod == null)
                HideMod();
            else
                ShowMod(SelectedMod);
        }

        private void modsCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_disableChecking)
                e.NewValue = e.CurrentValue;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            WizardForm.Attach(_menu);
        }

        private void parkitectNexusLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (SelectedMod == null) return;
            Process.Start($"https://client.parkitectnexus.com/redirect/{SelectedMod.Repository}");
        }

        private async void updateButton_Click(object sender, EventArgs e)
        {
            if (SelectedMod == null) return;
            WizardForm.Cursor = Cursors.WaitCursor;
            Enabled = false;

            try
            {
                var url = new ParkitectNexusUrl(SelectedMod.Name, ParkitectAssetType.Mod, SelectedMod.Repository);
                var info = await _parkitectOnlineAssetRepository.ResolveDownloadInfo(url);

                WizardForm.Cursor = Cursors.Default;
                Enabled = true;

                if (info.Tag == SelectedMod.Tag)
                {
                    MessageBox.Show(this, $"{SelectedMod} is already up to date!", "ParkitectNexus Client",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    WizardForm.Attach(new InstallAssetUserControl(_parkitect, _parkitectOnlineAssetRepository, _logger,
                        url, this));
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Failed to check for updates. Please try again later.", "ParitectNexus Client",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void uninstallButton_Click(object sender, EventArgs e)
        {
            if (SelectedMod == null) return;

            WizardForm.Cursor = Cursors.WaitCursor;
            Enabled = false;
            SelectedMod.Delete();
            await Task.Delay(500);

            FillListBox();
            WizardForm.Cursor = Cursors.Default;
            Enabled = true;
        }

        private void enableModCheckBox_Click(object sender, EventArgs e)
        {
            if (SelectedMod == null) return;
            SelectedMod.IsEnabled = enableModCheckBox.Checked;
            SelectedMod.Save();

            _disableChecking = false;
            modsCheckedListBox.SetItemChecked(modsCheckedListBox.SelectedIndex, SelectedMod.IsEnabled);
            _disableChecking = true;
        }
    }
}