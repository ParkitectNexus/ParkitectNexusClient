// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Diagnostics;
using System.IO;
using ParkitectNexus.Data.Game.Base;
using ParkitectNexus.Data.Settings;
using ParkitectNexus.Data.Settings.Models;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Game.MacOSX
{
    /// <summary>
    ///     Represents the Parkitect game on a MacOSX device.
    /// </summary>
    public class MacOSXParkitect : BaseParkitect
    {
        public MacOSXParkitect(ISettingsRepository<GameSettings> gameSettingsRepository, ILogger logger)
            : base(gameSettingsRepository, logger)
        {
            Paths = new MacOSXParkitectPaths(this);
        }

        /// <summary>
        ///     Gets a collection of paths.
        /// </summary>
        public override IParkitectPaths Paths { get; }

        /// <summary>
        ///     Detects the installation path.
        /// </summary>
        /// <returns>true if the installation path has been detected; false otherwise.</returns>
        public override bool DetectInstallationPath()
        {
            if (IsInstalled)
                return true;

            // TODO: Detect Steam version of the game

            return SetInstallationPathIfValid("/Applications/Parkitect.app");
        }

        /// <summary>
        ///     Launches the game.
        /// </summary>
        public override void Launch()
        {
            Logger.WriteLine($"Attempting to launch game.");

            if (IsInstalled)
                Process.Start(new ProcessStartInfo(
                    "open",
                    "-a '" + InstallationPath + "' --args -single-instance")
                {UseShellExecute = false});
        }

        protected override bool IsValidInstallationPath(string path)
        {
            // Path must exist and contain Contents/MacOS/Parkitect.
            return !string.IsNullOrWhiteSpace(path) && Directory.Exists(path) &&
                   File.Exists(Path.Combine(path, "Contents/MacOS/Parkitect"));
        }
    }
}
