// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Drawing;
using System.IO;

namespace ParkitectNexus.Data.Game
{
    public interface IParkitectAsset
    {
        ParkitectAssetType Type { get; }
        string InstallationPath { get; }
        string Name { get; }
        Image Thumbnail { get; }
        Stream Open();
    }
}
