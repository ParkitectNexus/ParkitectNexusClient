// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using System.Reflection;

namespace ParkitectNexus.Data.Utilities
{
    /// <summary>
    ///     Contains the path to the app data storage.
    /// </summary>
    public static class AppData
    {
        /// <summary>
        ///     Gets the path to the app data storage.
        /// </summary>
        public static string Path
        {
            get
            {
                var path = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    Assembly.GetEntryAssembly().GetName().Name);

                Directory.CreateDirectory(path);
                return path;
            }
        }
    }
}
