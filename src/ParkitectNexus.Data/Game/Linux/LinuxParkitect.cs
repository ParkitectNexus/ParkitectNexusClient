using System;
using ParkitectNexus.Data.Base;
using ParkitectNexus.Data.Game;
using System.Diagnostics;
using ParkitectNexus.Data.Utilities;
using System.Linq;
using System.IO;

namespace ParkitectNexus.Data
{
    public class LinuxParkitect : BaseParkitect
    {

        public LinuxParkitect()
        {
            Paths = new LinuxParkitectPath(this);
        }


        ///<summary>
        /// get a collection of paths
        /// </summary>
        public override IParkitectPaths Paths {get;}

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
                
                return running;
            }

            var t = Log.LoggingPath;
            Log.WriteLine($"Launching game at path '{Paths.GetPathInGameFolder("Parkitect.x86_64")}'.");
            // Start the game process.
            return !IsInstalled
                ? null
                    : Process.Start(new ProcessStartInfo(Paths.GetPathInGameFolder("Parkitect.x86_64"))
                        {
                            WorkingDirectory = InstallationPath,
                            Arguments = arguments
                        });
        }

        protected override bool IsValidInstallationPath(string path)
        {
            
            // Path must exist and contain Parkitect.exe.
            return !string.IsNullOrWhiteSpace(path) && File.Exists(Path.Combine(path, "Parkitect.x86_64"));
        }
    }
}

