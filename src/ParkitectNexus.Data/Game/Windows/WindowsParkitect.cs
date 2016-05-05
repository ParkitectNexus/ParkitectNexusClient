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

using System.Diagnostics;
using System.IO;
using System.Linq;
using ParkitectNexus.Data.Game.Base;
using ParkitectNexus.Data.Settings;
using ParkitectNexus.Data.Settings.Models;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Game.Windows
{
    /// <summary>
    ///     Represents the Parkitect game on a Windows device.
    /// </summary>
    public class WindowsParkitect : BaseParkitect
    {
        private readonly SteamPathSeeker _steamPathSeeker = new SteamPathSeeker();

        public WindowsParkitect(ISettingsRepository<GameSettings> gameSettingsRepository, ILogger logger)
            : base(gameSettingsRepository, logger)
        {
            Paths = new WindowsParkitectPaths(this);
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
            if (IsInstalled && GameSettings.Model.IsSteamVersion == _steamPathSeeker.IsSteamVersionInstalled)
                return true;

            var steamGamePath = _steamPathSeeker.GetParkitectInstallationPath();

            GameSettings.Model.IsSteamVersion = steamGamePath != null;
            GameSettings.Save();

            if (steamGamePath != null)
            {
                InstallationPath = steamGamePath;
            }

            return IsInstalled;
        }

        /// <summary>
        ///     Launches the game.
        /// </summary>
        public override void Launch()
        {
            Logger.WriteLine("Attempting to launch game.");

            // If the process is already running, push it to the foreground and return it.
            var running = Process.GetProcessesByName("Parkitect").FirstOrDefault();

            if (running != null)
            {
                Logger.WriteLine(
                    $"'Parkitect' is already running. Giving window handle '{running.MainWindowHandle}' focus.");

                User32.SetForegroundWindow(running.MainWindowHandle);
                return;
            }

            if (GameSettings.Model.IsSteamVersion)
            {
                Logger.WriteLine("Launching steam version of game.");
                Process.Start(Steam.LaunchUrl);
            }
            else
            {
                Logger.WriteLine($"Launching game at path '{Paths.GetPathInGameFolder("Parkitect.exe")}'.");
                // Start the game process.
                if (IsInstalled)
                    Process.Start(new ProcessStartInfo(Paths.GetPathInGameFolder("Parkitect.exe"))
                    {
                        WorkingDirectory = InstallationPath
                    });
            }
        }

        protected override bool IsValidInstallationPath(string path)
        {
            // Path must exist and contain Parkitect.exe.
            return !string.IsNullOrWhiteSpace(path) && File.Exists(Path.Combine(path, "Parkitect.exe"));
        }
    }
}