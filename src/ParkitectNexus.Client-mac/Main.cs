using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using ParkitectNexus.Data.Utilities;
using System.IO;
using ParkitectNexus.Data.Game.MacOSX;

namespace ParkitectNexus.Client
{
    class MainClass
    {
        static void Main (string[] args)
        {
            Log.Open(Path.Combine(AppData.Path, "ParkitectNexusLauncher.log"));
            Log.MinimumLogLevel = LogLevel.Debug;
            Log.WriteLine($"Starting with args '{string.Join(" ", args)}'");

            NSApplication.Init ();

            ModLoaderUtil.InstallModLoader(new MacOSXParkitect());

            NSApplication.Main (args);
        }
    }
}

