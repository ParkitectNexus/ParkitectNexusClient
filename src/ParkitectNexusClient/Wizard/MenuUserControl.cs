// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Threading;
using ParkitectNexus.Data;

namespace ParkitectNexus.Client.Wizard
{
    internal partial class MenuUserControl : BaseWizardUserControl
    {
        private readonly Parkitect _parkitect;
        private readonly ParkitectNexusWebsite _parkitectNexusWebsite;

        public MenuUserControl(Parkitect parkitect, ParkitectNexusWebsite parkitectNexusWebsite)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (parkitectNexusWebsite == null) throw new ArgumentNullException(nameof(parkitectNexusWebsite));
            _parkitect = parkitect;
            _parkitectNexusWebsite = parkitectNexusWebsite;
            InitializeComponent();
        }

        private void manageModsButton_Click(object sender, EventArgs e)
        {
            WizardForm.Attach(new ManageModsUserControl(this, _parkitect, _parkitectNexusWebsite));
        }

        private void launchParkitectButton_Click(object sender, EventArgs e)
        {
            WizardForm.Close();
            _parkitect.LaunchWithMods();
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