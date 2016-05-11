// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Reflection;
using Microsoft.Win32;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client.Win32
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
                // Find the path the the client application.
                var appPath = Assembly.GetEntryAssembly().Location;

                // Create registry keys in the classes tree for the parkitectnexus:// protocol.
                var parkitectNexus = Registry.CurrentUser?.CreateSubKey(@"Software\Classes\parkitectnexus");
                parkitectNexus?.SetValue("", "ParkitectNexus Client");
                parkitectNexus?.SetValue("URL Protocol", "");
                parkitectNexus?.CreateSubKey(@"DefaultIcon")?.SetValue("", $"{appPath},0");
                parkitectNexus?.CreateSubKey(@"shell\open\command")?.SetValue("", $"\"{appPath}\" --url \"%1\"");
            }
            catch (Exception e)
            {
                logger.WriteLine("Failed to install parkitectnexus:// protocol.", LogLevel.Fatal);
                logger.WriteException(e);
            }
        }
    }
}
