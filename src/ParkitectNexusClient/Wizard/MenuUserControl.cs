// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Reporting;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;

namespace ParkitectNexus.Client.Wizard
{
    internal partial class MenuUserControl : BaseWizardUserControl
    {
        private readonly ICrashReporterFactory _crashReporting;
        private readonly ILogger _logger;
        private readonly IParkitect _parkitect;
        private readonly IParkitectNexusWebsite _parkitectNexusWebsite;
        private readonly IParkitectOnlineAssetRepository _parkitectOnlineAssetRepository;

        public MenuUserControl(IParkitect parkitect, IParkitectNexusWebsite parkitectNexusWebsite,
            IParkitectOnlineAssetRepository parkitectOnlineAssetRepository, ICrashReporterFactory crashReporting,
            ILogger logger)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (parkitectNexusWebsite == null) throw new ArgumentNullException(nameof(parkitectNexusWebsite));
            if (parkitectOnlineAssetRepository == null)
                throw new ArgumentNullException(nameof(parkitectOnlineAssetRepository));
            _crashReporting = crashReporting;
            _logger = logger;
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
                        MessageBox.Show(this, "Are you sure you wish to send us your log files?",
                            "ParkitectNexus Client",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        _crashReporting.Report("log_upload", new Exception("log_upload"));

                    return true;
                case (Keys.Control | Keys.Alt | Keys.D):
                    _logger.Close();
                    WizardForm.Close();

                    var path = Assembly.GetEntryAssembly().Location;
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
            WizardForm.Attach(new ManageModsUserControl(this, _parkitect, _parkitectOnlineAssetRepository, _logger));
        }

        private void launchParkitectButton_Click(object sender, EventArgs e)
        {
            WizardForm.Hide();
            _parkitect.Launch();
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