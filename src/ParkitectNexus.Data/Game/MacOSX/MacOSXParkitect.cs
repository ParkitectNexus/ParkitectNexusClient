// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System.Diagnostics;
using System.IO;
using ParkitectNexus.Data.Base;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Game.MacOSX
{
    /// <summary>
    ///     Represents the Parkitect game on a MacOSX device.
    /// </summary>
    public class MacOSXParkitect : BaseParkitect
    {
        public MacOSXParkitect()
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

            return SetInstallationPathIfValid("/Applications/Parkitect.app");
        }

        /// <summary>
        ///     Launches the game with the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The launched process.</returns>
        public override Process Launch(string arguments = "-single-instance")
        {
            Log.WriteLine($"Attempting to launch game with arguments '{arguments}'.");

            Log.WriteLine("Attempting to compile installed mods.");
            CompileActiveMods();

            return IsInstalled
                ? Process.Start(new ProcessStartInfo(
                    "open",
                    "-a '" + InstallationPath + "' --args " + arguments)
                {UseShellExecute = false})
                : null;
        }

        protected override bool IsValidInstallationPath(string path)
        {
            // Path must exist and contain Contents/MacOS/Parkitect.
            return !string.IsNullOrWhiteSpace(path) && Directory.Exists(path) &&
                   File.Exists(Path.Combine(path, "Contents/MacOS/Parkitect"));
        }
    }
}