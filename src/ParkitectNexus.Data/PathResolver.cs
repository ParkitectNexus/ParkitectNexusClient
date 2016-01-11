// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using System.Reflection;

namespace ParkitectNexus.Data
{
    public class PathResolver : IPathResolver
    {
        public string AppData()
        {
            var path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Assembly.GetEntryAssembly().GetName().Name);

            Directory.CreateDirectory(path);
            return path;
        }

        public bool IsParkitectInstalled()
        {
            throw new NotImplementedException();
        }

        public string ParkitectPath()
        {
            throw new NotImplementedException();
        }
    }
}