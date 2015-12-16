using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using ParkitectNexus.Data.Utilities;
using System.IO;
using ParkitectNexus.Data.Game.MacOSX;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Reporting;

namespace ParkitectNexus.Client
{
    class MainClass
    {
        static void Main (string[] args)
        {
            Log.Open(Path.Combine(AppData.Path, "ParkitectNexusLauncher.log"));
            Log.MinimumLogLevel = LogLevel.Debug;
            Log.WriteLine($"Starting with args '{string.Join(" ", args)}'");

            var parkitect = new MacOSXParkitect();
            parkitect.DetectInstallationPath();
            var parkitectNexusWebsite = new ParkitectNexusWebsite();

            #if !DEBUG
            try
            {
            #endif
                NSApplication.Init ();

                try
                {
                    ModLoaderUtil.InstallModLoader(parkitect);
                }
                catch(Exception e)
                {
                    Log.WriteLine("Failed to install mod loader", LogLevel.Fatal);
                    Log.WriteException(e);
                    CrashReporter.Report("install_mod_loader", parkitect, parkitectNexusWebsite, e);
                }
                NSApplication.Main (args);
            #if !DEBUG
            }
            catch(Exception e)
            {
                Log.WriteLine("Application exited in an unusual way.", LogLevel.Fatal);
                Log.WriteException(e);
                CrashReporter.Report("global", parkitect, parkitectNexusWebsite, e);
            }
            #endif
        }
    }
}

