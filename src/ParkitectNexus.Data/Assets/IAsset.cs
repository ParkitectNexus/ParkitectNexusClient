// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Data.Assets
{
    public interface IAsset
    {
        string Name { get; }
        string InstallationPath { get; }
        AssetType Type { get; }
        AssetCachedData CachedData { get; }

        Task<Image> GetThumbnail();
        Task<Image> GetImage();

        Stream Open();
    }
}
