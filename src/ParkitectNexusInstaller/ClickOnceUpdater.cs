// ParkitectNexusInstaller
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Deployment.Application;
using System.Windows.Forms;

namespace ParkitectNexusInstaller
{
    public static class ClickOnceUpdater
    {
        public static void Update()
        {
            if (!ApplicationDeployment.IsNetworkDeployed) return;

            try
            {
                var info = ApplicationDeployment.CurrentDeployment.CheckForDetailedUpdate();

                if (info == null || !info.UpdateAvailable || !info.IsUpdateRequired)
                    return;

                ApplicationDeployment.CurrentDeployment.Update();
                Application.Restart();
            }
            catch (Exception)
            {
            }
        }

        public static void UpdateSettings()
        {
            Properties.Settings.Default.Upgrade();
            Properties.Settings.Default.IsFirstRun = false;
            Properties.Settings.Default.Save();
        }
    }
}