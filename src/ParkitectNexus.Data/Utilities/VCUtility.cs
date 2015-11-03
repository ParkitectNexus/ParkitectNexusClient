// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System.Diagnostics;
using Microsoft.Win32;

namespace ParkitectNexus.Data.Utilities
{
    public static class VCUtility
    {
        public static bool IsInstalled(string version)
        {
            var key =
                Registry.LocalMachine.OpenSubKey(
                    $"SOFTWARE\\Microsoft\\DevDiv\\vc\\Servicing\\{version}\\RuntimeMinimum");
            var install = key?.GetValue("Install");
            return key != null && install != null && (int)install == 1;
        }
    }
}