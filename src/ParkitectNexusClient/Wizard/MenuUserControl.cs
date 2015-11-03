// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client.Wizard
{
    internal partial class MenuUserControl : BaseWizardUserControl
    {
        private readonly Parkitect _parkitect;
        private readonly ParkitectNexusWebsite _parkitectNexusWebsite;
        private readonly ParkitectOnlineAssetRepository _parkitectOnlineAssetRepository;

        public MenuUserControl(Parkitect parkitect, ParkitectNexusWebsite parkitectNexusWebsite,
            ParkitectOnlineAssetRepository parkitectOnlineAssetRepository)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (parkitectNexusWebsite == null) throw new ArgumentNullException(nameof(parkitectNexusWebsite));
            if (parkitectOnlineAssetRepository == null)
                throw new ArgumentNullException(nameof(parkitectOnlineAssetRepository));

            _parkitect = parkitect;
            _parkitectNexusWebsite = parkitectNexusWebsite;
            _parkitectOnlineAssetRepository = parkitectOnlineAssetRepository;

            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case (Keys.Control | Keys.Alt | Keys.Shift | Keys.C):
                    if (
                        MessageBox.Show(this, "Are you sure you wish to send us your log files?", "ParkitectNexus Client",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        CrashReporter.Report("log_upload", _parkitect, _parkitectNexusWebsite, new Exception("log_upload"));

                    return true;
                case (Keys.Control | Keys.Alt | Keys.D):
                    Log.Close();
                    WizardForm.Close();

                    var path = System.Reflection.Assembly.GetEntryAssembly().Location;
                    Process.Start(path, "--loglevel Debug");
                    break;
                case (Keys.Control | Keys.Alt | Keys.L):
                    Process.Start(AppData.Path);
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void manageModsButton_Click(object sender, EventArgs e)
        {
            WizardForm.Attach(new ManageModsUserControl(this, _parkitect, _parkitectOnlineAssetRepository));
        }

        private void launchParkitectButton_Click(object sender, EventArgs e)
        {
            WizardForm.Hide();
            LaunchUtil.LaunchWithModsAndCrashHandler(_parkitect, _parkitectNexusWebsite, "menu");
            WizardForm.Close();
        }

        private void visitParkitectNexusButton_Click(object sender, EventArgs e)
        {
            _parkitectNexusWebsite.Launch();
            WizardForm.Close();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            WizardForm.Close();
        }
    }
}