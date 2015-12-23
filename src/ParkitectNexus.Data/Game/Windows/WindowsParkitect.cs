// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System.Diagnostics;
using System.IO;
using System.Linq;
using ParkitectNexus.Data.Base;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Game.Windows
{
    /// <summary>
    ///     Represents the Parkitect game on a Windows device.
    /// </summary>
    public class WindowsParkitect : BaseParkitect
    {
        public WindowsParkitect()
        {
            Paths = new WindowsParkitectPaths(this);
        }


		///<summary>
		/// get a collection of paths
		/// </summary>
		public override IParkitectPaths Paths {get;protected set;}

        /// <summary>
        ///     Detects the installation path.
        /// </summary>
        /// <returns>true if the installation path has been detected; false otherwise.</returns>
        public override bool DetectInstallationPath()
        {
            if (IsInstalled)
                return true;

            // TODO Detect registry key of installation path.
            // can only do this once it's installed trough steam or a setup.
            return false;
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

            // If the process is already running, push it to the foreground and return it.
            var running = Process.GetProcessesByName("Parkitect").FirstOrDefault();

            if (running != null)
            {
                Log.WriteLine(
                    $"'Parkitect' is already running. Giving window handle '{running.MainWindowHandle}' focus.");

                User32.SetForegroundWindow(running.MainWindowHandle);
                return running;
            }

            Log.WriteLine($"Launching game at path '{Paths.GetPathInGameFolder("Parkitect.exe")}'.");
            // Start the game process.
            return !IsInstalled
                ? null
                    : Process.Start(new ProcessStartInfo(Paths.GetPathInGameFolder("Parkitect.exe"))
                {
                    WorkingDirectory = InstallationPath,
                    Arguments = arguments
                });
        }
        
        protected override bool IsValidInstallationPath(string path)
        {
            // Path must exist and contain Parkitect.exe.
            return !string.IsNullOrWhiteSpace(path) && File.Exists(Path.Combine(path, "Parkitect.exe"));
        }
    }
}