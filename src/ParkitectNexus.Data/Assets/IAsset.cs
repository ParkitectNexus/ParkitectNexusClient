// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Drawing;
using System.IO;

namespace ParkitectNexus.Data.Assets
{
    public interface IAsset
    {
        string Id { get; }

        DateTime InstalledVersion { get; }

        string Name { get; }

        string InstallationPath { get; }

        AssetType Type { get; }
        
        Image GetImage();
        Stream Open();
    }
}
