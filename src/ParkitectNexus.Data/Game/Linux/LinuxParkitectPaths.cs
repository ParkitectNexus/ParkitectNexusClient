// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.IO;
using ParkitectNexus.Data.Game.Base;

namespace ParkitectNexus.Data.Game.Linux
{
    public class LinuxParkitectPaths : BaseParkitectPaths
    {
        public LinuxParkitectPaths(LinuxParkitect parkitect) : base(parkitect)
        {
        }

        public override string Data => GetPathInGameFolder("Parkitect_Data");

        public override string DataManaged => GetPathInGameFolder(@"Parkitect_Data/Managed");

        public override string GetPathInSavesFolder(string path, bool createIfNotExists)
        {
            if (!Parkitect.IsInstalled)
                return null;

            path = path == null
                ? Parkitect.InstallationPath
                : Path.Combine(Parkitect.InstallationPath, path);

            if (path != null && createIfNotExists)
                Directory.CreateDirectory(path);

            return path;
        }
    }
}
