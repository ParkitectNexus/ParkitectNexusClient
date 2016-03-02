using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Presenter;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System;


namespace ParkitectNexus.Client.Linux
{
    public class ProtocalInstallUtility : IPresenter
    {
        public ProtocalInstallUtility ()
        {
			if (ParkitectNexus.Data.Utilities.OperatingSystem.Detect() == Data.SupportedOperatingSystem.Linux)
            {
                var appPath = Assembly.GetEntryAssembly().Location;
                var s = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/applications/", "parkitectnexus.desktop");
                File.WriteAllText(s, "[Desktop Entry] \n" +
                    "Comment[en_US]= \n" +
                    "Comment= \n" +
                    "Exec=mono " + appPath + " --url %u \n" +
                    "GenericName[en_US]= \n" +
                    "GenericName= \n" +
                    "Icon=" + Directory.GetCurrentDirectory() + "/parkitectnexus_logo.png" + " \n" +
                    "Name=Parkitect Nexus \n" +
                    "Path=" + appPath + "\n" +
                    "StartupNotify=true \n" +
                    "Terminal=false \n" +
                    "TerminalOptions= \n" +
                    "Type=Application \n" +
                    "MimeType=x-scheme-handler/parkitectnexus;");
                Process.Start("update-desktop-database");
           
            }

        }
    }
}

