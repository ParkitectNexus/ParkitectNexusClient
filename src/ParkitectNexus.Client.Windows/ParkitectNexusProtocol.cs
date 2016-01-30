// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Reflection;
using Microsoft.Win32;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client.Windows
{
    public static class ParkitectNexusProtocol
    {
        /// <summary>
        ///     Installs the parkitectnexus:// protocol.
        /// </summary>
        public static void Install(ILogger logger)
        {
            try
            {
                var appPath = Assembly.GetEntryAssembly().Location;

                var parkitectNexus = Registry.CurrentUser?.CreateSubKey(@"Software\Classes\parkitectnexus");
                parkitectNexus?.SetValue("", "ParkitectNexus Client");
                parkitectNexus?.SetValue("URL Protocol", "");
                parkitectNexus?.CreateSubKey(@"DefaultIcon")?.SetValue("", $"{appPath},0");
                parkitectNexus?.CreateSubKey(@"shell\open\command")?.SetValue("", $"\"{appPath}\" --url \"%1\"");
            }
            catch (Exception e)
            {
                logger.WriteLine("Failed to install parkitectnexus:// protocol.");
                logger.WriteException(e);
            }
        }
    }
}
