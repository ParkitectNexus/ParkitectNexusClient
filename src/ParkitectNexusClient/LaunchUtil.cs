// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client
{
    public static class LaunchUtil
    {
        public static void LaunchWithModsAndCrashHandler(Parkitect parkitect,
            ParkitectNexusWebsite parkitectNexusWebsite, string launchLocation)
        {
            try
            {
                if (!VCUtility.IsInstalled("14.0"))
                {
                    using (var focus = new FocusForm())
                    {
                        if (MessageBox.Show(focus,
                            "In order to launch Parkitect with mods you need to install Visual C++ 2015.\nWould you like to install it now?",
                            "ParkitectNexus Client", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) ==
                            DialogResult.Yes)
                        {
                            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                                "redist/vc_redist.x86.exe");
                            if (!File.Exists(path))
                            {
                                MessageBox.Show("Could not find redist/vc_redist.x86.exe. File is missing?",
                                    "ParkitectNexus Client", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                                return;
                            }

                            Process.Start(path, "/norestart");
                        }
                    }

                    return;
                }

                parkitect.LaunchWithMods();
            }
            catch (Exception e)
            {
                Log.WriteLine("Launching failed in an unusual way.", LogLevel.Fatal);
                Log.WriteException(e);
                CrashReporter.Report("launch_from_" + launchLocation, parkitect, parkitectNexusWebsite, e);

                using (var focus = new FocusForm())
                {
                    MessageBox.Show(focus,
                        $"Launching the game with mods failed.\n\nThe error has been logged to:\n{Log.LoggingPath}",
                        "ParkitectNexus Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}