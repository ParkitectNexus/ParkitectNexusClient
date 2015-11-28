// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.IO;

namespace ParkitectNexus.Data.MacOSX
{
    /// <summary>
    ///     Represents a paths collection for the Windows version of the Parkitect game.
    /// </summary>
    public class MacOSXParkitectPaths : IParkitectPaths
    {
        private readonly MacOSXParkitect _parkitect;

		public MacOSXParkitectPaths(MacOSXParkitect parkitect)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            _parkitect = parkitect;
        }

        public string Installation => GetPathInGameFolder(null);
        public string Data => GetPathInGameFolder("Contents/Resources/Data");
        public string DataManaged => GetPathInGameFolder("Contents/Resources/Data/Managed");
        public string Mods => GetPathInSavesFolder("pnmods", true);
        public string NativeMods => GetPathInSavesFolder("Mods", true);

        public string GetPathInSavesFolder(string path)
        {
            return GetPathInSavesFolder(path, false);
        }

        public string GetPathInSavesFolder(string path, bool createIfNotExists)
        {
            
            path = !_parkitect.IsInstalled
                ? null
                : path == null
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library/Application Support/Parkitect")
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library/Application Support/Parkitect", path);

            if (path != null && createIfNotExists)
                Directory.CreateDirectory(path);

            return path;
        }

        public string GetPathInGameFolder(string path)
        {
            return GetPathInGameFolder(path, false);
        }

        public string GetPathInGameFolder(string path, bool createIfNotExists)
        {
            path = !_parkitect.IsInstalled
                ? null
                : path == null ? _parkitect.InstallationPath : Path.Combine(_parkitect.InstallationPath, path);

            if (path != null && createIfNotExists)
                Directory.CreateDirectory(path);

            return path;
        }
    }
}