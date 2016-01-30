// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Data.Assets
{
    public class Asset : IAsset
    {
        public Asset(string path, AssetCachedData data, AssetType type)
        {
            InstallationPath = path;
            CachedData = data;
            Type = type;
        }

        #region Implementation of IAsset

        public string Name => CachedData.Name;
        public string InstallationPath { get; }
        public AssetType Type { get; }
        public AssetCachedData CachedData { get; }

        public virtual Task<Image> GetThumbnail()
        {
            return Task.FromResult(CachedData.GetThumbnail());
        }

        public virtual Task<Image> GetImage()
        {
            return Task.FromResult(CachedData.GetImage());
        }

        public virtual Stream Open()
        {
            return File.OpenRead(InstallationPath);
        }

        #endregion
    }
}
