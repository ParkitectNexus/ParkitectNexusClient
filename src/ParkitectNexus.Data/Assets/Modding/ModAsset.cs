// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
        private readonly AssetWithImageCachedData _cachedData;
        private readonly IModMetadata _metadata;

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

        #region Overrides of Asset

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return base.ToString() + $", Enabled: {Information.IsEnabled}, Tag: {Tag}, Repository: {Repository}";
        }

        #endregion

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
    }
}