// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Drawing;
using System.IO;
using ParkitectNexus.Data.Assets.CachedData;
using ParkitectNexus.Data.Assets.Meta;

namespace ParkitectNexus.Data.Assets.Modding
{
    public interface IModAsset : IAsset
    {
        ModInformation Information { get; }
    }

    public class ModAsset : Asset, IModAsset
    {
        private readonly AssetWithImageCachedData _cachedData;

        public ModAsset(string path, IAssetMetadata metadata, AssetWithImageCachedData cachedData,
            ModInformation information)
            : base(path, metadata, cachedData, AssetType.Mod)
        {
            if (cachedData == null) throw new ArgumentNullException(nameof(cachedData));
            _cachedData = cachedData;
            Information = information;
        }

        public ModInformation Information { get; }

        #region Overrides of Asset

        public override string Name => Information.Name;

        public override Image GetImage()
        {
            return _cachedData?.GetImage();
        }

        public override Stream Open()
        {
            throw new NotImplementedException("mods cannot be opened as a stream");
        }

        #endregion
    }
}