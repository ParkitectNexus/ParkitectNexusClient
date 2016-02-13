// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.IO;
using ParkitectNexus.Data.Game.Base;
using ParkitectNexus.Data.Game.Linux;

namespace ParkitectNexus.Data
{
    public class LinuxParkitectPath : BaseParkitectPaths
    {
        public LinuxParkitectPath(LinuxParkitect parkitect) : base(parkitect)
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