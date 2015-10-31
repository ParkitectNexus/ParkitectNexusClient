// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.IO;
using System.Reflection;

namespace ParkitectNexus.Data
{
    public static class AppData
    {
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