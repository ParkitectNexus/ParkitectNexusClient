// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using ParkitectNexus.Data.Assets.CachedData;
using ParkitectNexus.Data.Assets.Meta;

namespace ParkitectNexus.Data.Assets.Modding
{
    public class ModAsset : Asset, IModAsset
    {
        private readonly IModMetadata _metadata;
        private readonly AssetWithImageCachedData _cachedData;

        public ModAsset(string path, IModMetadata metadata, AssetWithImageCachedData cachedData,
            ModInformation information)
            : base(path, metadata, cachedData, AssetType.Mod)
        {
            if (cachedData == null) throw new ArgumentNullException(nameof(cachedData));
            if (information == null) throw new ArgumentNullException(nameof(information));
            _metadata = metadata;
            _cachedData = cachedData;
            Information = information;
        }

        #region Implementation of IModAsset

        public ModInformation Information { get; }

        public string Tag => _metadata?.Tag;
        public string Repository => _metadata?.Repository;

        public StreamWriter OpenLogFile()
        {
            var path = Path.Combine(InstallationPath, "mod.log");

            return new StreamWriter(File.OpenWrite(path));
        }

        public void SaveInformation()
        {
            var path = Path.Combine(InstallationPath, "mod.json");

            var data = JsonConvert.SerializeObject(Information);

            File.WriteAllText(path, data);
        }

        #endregion

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

        #region Overrides of Asset

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return base.ToString() + $", Enabled: {Information.IsEnabled}, Tag: {Tag}, Repository: {Repository}";
        }

        #endregion
    }
}
