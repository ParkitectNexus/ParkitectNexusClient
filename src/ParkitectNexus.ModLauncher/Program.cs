// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
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
            
            // Check to see if the parkitect game has been installed.
            if (!parkitect.DetectInstallationPath())
            {
                var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                while (!parkitect.SetInstallationPathIfValid(path))
                {
                    var newPath = Path.GetDirectoryName(path);

                    if (newPath == path || newPath == null)
                        return;
                    path = newPath;
                }
            }

            try
            {
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
                {
                    try
                    {
                        ModInjector.Inject(m.AssemblyPath, m.NameSpace, m.ClassName, m.MethodName);
                    }
                    catch (Exception e)
                    {
                        using (var logFile = m.OpenLog())
                        {
                            logFile.Log($"Failed to inject mod. {e.Message}", LogLevel.Fatal);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                using (
                    var logFile = File.AppendText(Path.Combine(parkitect.InstallationPath, "ParkitectModLauncher.log")))
                {
                    logFile.Log(e.Message, LogLevel.Fatal);
                }
            }

        }
    }
}