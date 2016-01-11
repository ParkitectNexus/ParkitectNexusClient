// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;

namespace ParkitectNexus.Data.Game.Base
{
    public abstract class BaseParkitectPaths : IParkitectPaths
    {
        protected BaseParkitectPaths(IParkitect parkitect)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            Parkitect = parkitect;
        }

        protected IParkitect Parkitect { get; }

        public virtual string Installation => GetPathInGameFolder(null);
        public abstract string Data { get; }
        public abstract string DataManaged { get; }
        public virtual string Mods => GetPathInSavesFolder("pnmods", true);
        public virtual string NativeMods => GetPathInSavesFolder("Mods", true);

        public string GetPathInSavesFolder(string path)
        {
            return GetPathInSavesFolder(path, false);
        }

        public abstract string GetPathInSavesFolder(string path, bool createIfNotExists);

        public string GetPathInGameFolder(string path)
        {
            return GetPathInGameFolder(path, false);
        }

        public virtual string GetPathInGameFolder(string path, bool createIfNotExists)
        {
            path = !Parkitect.IsInstalled
                ? null
                : path == null ? Parkitect.InstallationPath : Path.Combine(Parkitect.InstallationPath, path);

            if (path != null && createIfNotExists)
                Directory.CreateDirectory(path);

            return path;
        }
    }
}