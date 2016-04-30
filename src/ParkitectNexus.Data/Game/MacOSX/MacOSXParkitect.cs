// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
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
            if (IsInstalled && GameSettings.Model.IsSteamVersion)
                return true;

            bool success = SetInstallationPathIfValid(
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    "Library/Application Support/Steam/steamapps/common/Parkitect/Parkitect.app")) ||
                           SetInstallationPathIfValid("/Applications/Parkitect.app");

            if (IsInstalled)
            {
                GameSettings.Model.IsSteamVersion = File.Exists(Path.Combine(InstallationPath, "installscript_osx.vdf"));
                GameSettings.Save();
            }

            return success;
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