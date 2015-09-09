// ParkitectNexusInstaller
// Copyright 2015 Parkitect, Tim Potze

using ParkitectNexusInstaller.Properties;

namespace ParkitectNexusInstaller
{
    /// <summary>
    /// Contains utilities for updating.
    /// </summary>
    internal static class UpdateUtil
    {
        /// <summary>
        /// Migrates settings from previous versions.
        /// </summary>
        public static void MigrateSettings()
        {
            if (Settings.Default.IsFirstRun)
            {
                Settings.Default.Upgrade();
                Settings.Default.IsFirstRun = false;
                Settings.Default.Save();
            }
        }
    }
}