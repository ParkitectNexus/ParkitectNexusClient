using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using ParkitectNexus.Data.Utilities;
using System.IO;

namespace ParkitectNexus.Clientmac
{
    class MainClass
    {
        static void Main (string[] args)
        {
            Log.Open(Path.Combine(AppData.Path, "ParkitectNexusLauncher.log"));

            NSApplication.Init ();
            NSApplication.Main (args);
        }
    }
}

