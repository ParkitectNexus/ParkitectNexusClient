// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using ParkitectNexus.Data.Bases;

namespace ParkitectNexus.Data.Game.Windows
{
    /// <summary>
    ///     Represents a paths collection for the Windows version of the Parkitect game.
    /// </summary>
    public class WindowsParkitectPaths : BaseParkitectPaths
    {
        public WindowsParkitectPaths(IParkitect parkitect) : base(parkitect)
        {
        }

        public override string Data => GetPathInGameFolder("Parkitect_Data");
        public override string DataManaged => GetPathInGameFolder(@"Parkitect_Data\Managed");

        public override string GetPathInSavesFolder(string path, bool createIfNotExists)
        {
            string documentsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Parkitect");

            path = !Parkitect.IsInstalled
                ? null
                : path == null
                    ? Path.Combine(documentsFolder)
                    : Path.Combine(documentsFolder, path);

            if (path != null && createIfNotExists)
                Directory.CreateDirectory(path);

            return path;
        }
    }
}