// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using ParkitectNexus.Data;
using ParkitectNexus.ModLoader;

namespace ParkitectNexus.ModLauncher
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parkitect = new Parkitect();

#if DEBUG
            if (!parkitect.IsInstalled)
                parkitect.SetInstallationPathIfValid(@"C:\Users\Tim\Desktop\Parkitect - Modded");
#endif

            // Check to see if the parkitect game has been installed.
            if (!parkitect.DetectInstallationPath() &&
                !parkitect.SetInstallationPathIfValid(
                    Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)))
                return;

            // Compile mods.
            var mods = parkitect.InstalledMods.Where(mod => mod.Compile(parkitect)).ToArray();

            // Launch the game.
            var process = parkitect.Launch();

            // Wait for the game to start.
            do
            {
                Thread.Sleep(500);
                process.Refresh();
            } while (!process.HasExited && process.MainWindowTitle.Contains("Configuration"));

            // Make sure game didn't close.
            if (process.HasExited)
                return;

            // Inject mods.
            foreach (var m in mods)
                ModInjector.Inject(m.AssemblyPath, m.NameSpace, m.ClassName, m.MethodName);
        }
    }
}