// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Diagnostics;
using System.IO;
using System.Linq;
using ParkitectNexus.Data.Game.Base;
using ParkitectNexus.Data.Settings;
using ParkitectNexus.Data.Settings.Models;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Game.Linux
{
    public class LinuxParkitect : BaseParkitect
    {
        public LinuxParkitect(ISettingsRepository<GameSettings> gameSettingsRepository, ILogger logger)
            : base(gameSettingsRepository, logger)
        {
            Paths = new LinuxParkitectPaths(this);
        }


        /// <summary>
        ///     get a collection of paths
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

            // TODO Detect Steam version of the game

            return false;
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

                return;
            }

            Logger.WriteLine($"Launching game at path '{Paths.GetPathInGameFolder("Parkitect.x86_64")}'.");
            // Start the game process.
            if (IsInstalled)
                Process.Start(new ProcessStartInfo(Paths.GetPathInGameFolder("Parkitect.x86_64"))
                {
                    WorkingDirectory = InstallationPath
                });
        }

        protected override bool IsValidInstallationPath(string path)
        {
            // Path must exist and contain Parkitect.exe.
            return !string.IsNullOrWhiteSpace(path) && File.Exists(Path.Combine(path, "Parkitect.x86_64"));
        }
    }
}
