// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.IO;
using System.Linq;

namespace ParkitectNexus.Data
{
    /// <summary>
    ///     Represents a paths collection for the Parkitect game.
    /// </summary>
    public class ParkitectPaths
    {
        private readonly Parkitect _parkitect;

        public ParkitectPaths(Parkitect parkitect)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            _parkitect = parkitect;
        }

        public string Installation => GetPath();

        public string Data => GetPath("Parkitect_Data");
        public string DataManaged => GetPath("Parkitect_Data", "Managed");
        public string Mods => GetPath(true, "mods");
        public string Executable => GetPath("Parkitect.exe");

        private string GetPath(params string[] paths)
        {
            return GetPath(false, paths);
        }

        private string GetPath(bool createIfNotExists, params string[] paths)
        {
            var path = !_parkitect.IsInstalled
                ? null
                : Path.Combine(new[] {_parkitect.InstallationPath}.Concat(paths).ToArray());

            if (path != null && createIfNotExists)
                Directory.CreateDirectory(path);

            return path;
        }
    }
}