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
    internal partial class ManageModsUserControl : BaseWizardUserControl
    {
        private readonly MenuUserControl _menu;
        private readonly Parkitect _parkitect;
        private readonly ParkitectNexusWebsite _parkitectNexusWebsite;

        public ManageModsUserControl(MenuUserControl menu, Parkitect parkitect, ParkitectNexusWebsite parkitectNexusWebsite)
        {
            if (menu == null) throw new ArgumentNullException(nameof(menu));
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (parkitectNexusWebsite == null) throw new ArgumentNullException(nameof(parkitectNexusWebsite));

            _menu = menu;
            _parkitect = parkitect;
            _parkitectNexusWebsite = parkitectNexusWebsite;
            InitializeComponent();
        }

        #region Overrides of BaseWizardUserControl

        protected override void OnAttached()
        {
            FillListBox();
            base.OnAttached();
        }

        #endregion

        private ParkitectMod SelectedMod => modsCheckedListBox.SelectedItem as ParkitectMod;

        private bool _disableChecking = true;
        private void FillListBox()
        {
            modsCheckedListBox.Items.Clear();
            modsCheckedListBox.Items.AddRange(_parkitect.InstalledMods.ToArray());

            _disableChecking = false;
            for (var i=0;i< modsCheckedListBox.Items.Count;i++)
                modsCheckedListBox.SetItemChecked(i, ((ParkitectMod)modsCheckedListBox.Items[i]).IsEnabled);
            _disableChecking = true;

            optionsGroupBox.Enabled = false;
            modNameLabel.Text = "-";
            modVersionLabel.Text = "-";
            enableModCheckBox.Checked = false;
            modsCheckedListBox.SelectedItem = null;
        }

        private void modsCheckedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedMod == null)
            {
                optionsGroupBox.Enabled = false;
                modNameLabel.Text = "-";
                modVersionLabel.Text = "-";
                enableModCheckBox.Checked = false;
            }
            else
            {
                ShowMod(SelectedMod);
            }
        }

        private void ShowMod(ParkitectMod mod)
        {
            optionsGroupBox.Enabled = true;
            modNameLabel.Text = mod.Name;
            modVersionLabel.Text = mod.Tag;
            enableModCheckBox.Checked = mod.IsEnabled;
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
        }

        private async void updateButton_Click(object sender, EventArgs e)
        {
            if (SelectedMod == null) return;
            WizardForm.Cursor = Cursors.WaitCursor;
            Enabled = false;

            var url = new ParkitectNexusUrl(SelectedMod.Name, ParkitectAssetType.Mod, SelectedMod.Repository);
            var info = await _parkitectNexusWebsite.ResolveDownloadUrl(url);
            
            WizardForm.Cursor = Cursors.Default;
            Enabled = true;

            if (info.Tag == SelectedMod.Tag)
            {
                MessageBox.Show(this, $"{SelectedMod} is already up to date!", "ParkitectNexus Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                WizardForm.Attach(new InstallAssetUserControl(_parkitect, _parkitectNexusWebsite, url, this));
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
