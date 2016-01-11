// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using ParkitectNexus.Data.Game.Base;

namespace ParkitectNexus.Data.Game.MacOSX
{
    /// <summary>
    ///     Represents a paths collection for the MacOSX version of the Parkitect game.
    /// </summary>
    public class MacOSXParkitectPaths : BaseParkitectPaths
    {
        public MacOSXParkitectPaths(MacOSXParkitect parkitect) : base(parkitect)
        {
        }

        public override string Data => GetPathInGameFolder("Contents/Resources/Data");
        public override string DataManaged => GetPathInGameFolder("Contents/Resources/Data/Managed");

        public override string GetPathInSavesFolder(string path, bool createIfNotExists)
        {
            if (!Parkitect.IsInstalled)
                return null;

            var parkitectFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "Library/Application Support/Parkitect");

            path = path == null
                    ? parkitectFolder
                    : Path.Combine(parkitectFolder, path);

            if (path != null && createIfNotExists)
                Directory.CreateDirectory(path);

            return path;
        }
    }
}
