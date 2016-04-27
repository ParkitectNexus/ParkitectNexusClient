// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

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
        private SteamPathSeeker _steamPathSeeker = new SteamPathSeeker();

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
            if(IsInstalled && GameSettings.Model.IsSteamVersion == _steamPathSeeker.IsSteamVersionInstalled)
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
