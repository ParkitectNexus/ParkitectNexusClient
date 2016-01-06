// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Reflection;
using Microsoft.Win32;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client
{
    public static class ParkitectNexusProtocol
    {
        /// <summary>
        ///     Installs the parkitectnexus:// protocol.
        /// </summary>
        public static void Install()
        {
            try
            {
                var appPath = Assembly.GetEntryAssembly().Location;

                var parkitectNexus = Registry.CurrentUser?.CreateSubKey(@"Software\Classes\parkitectnexus");
                parkitectNexus?.SetValue("", "ParkitectNexus Client");
                parkitectNexus?.SetValue("URL Protocol", "");
                parkitectNexus?.CreateSubKey(@"DefaultIcon")?.SetValue("", $"{appPath},0");
                parkitectNexus?.CreateSubKey(@"shell\open\command")?.SetValue("", $"\"{appPath}\" --download \"%1\"");
            }
            catch (Exception e)
            {
                Log.WriteLine("Failed to install parkitectnexus:// protocol.");
                Log.WriteException(e);
            }
        }
    }
}