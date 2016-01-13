// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Drawing;
using System.IO;

namespace ParkitectNexus.Data.Game
{
    public class ParkitectAsset : IParkitectAsset
    {

        public ParkitectAsset(string name, string installationPath, Image thumbnail, ParkitectAssetType type)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (installationPath == null) throw new ArgumentNullException(nameof(installationPath));
            Thumbnail = thumbnail;
            Name = name;
            InstallationPath = installationPath;
            Type = type;
        }

        public ParkitectAssetType Type { get; }
        public string InstallationPath { get; }

        public string Name { get; }
        public Image Thumbnail { get; }

        public Stream Open()
        {
            return File.OpenRead(InstallationPath);
        }
    }
}
