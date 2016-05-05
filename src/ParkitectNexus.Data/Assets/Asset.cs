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
using ParkitectNexus.Data.Assets.CachedData;
using ParkitectNexus.Data.Assets.Meta;

namespace ParkitectNexus.Data.Assets
{
    public abstract class Asset : IAsset
    {
        private readonly IAssetCachedData _cachedData;
        private readonly IAssetMetadata _metadata;

        protected Asset(string path, IAssetMetadata metadata, IAssetCachedData cachedData, AssetType type)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (cachedData == null) throw new ArgumentNullException(nameof(cachedData));

            InstallationPath = path;
            _metadata = metadata;
            _cachedData = cachedData;
            Type = type;
        }

        #region Overrides of Object

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return $"{Type}Asset, Name: {Name}";
        }

        #endregion

        #region Implementation of IAsset

        public string Id => _metadata?.Id;

        public DateTime InstalledVersion => _metadata?.InstalledVersion ?? default(DateTime);

        public virtual string Name => _cachedData.Name;

        public virtual string InstallationPath { get; }

        public virtual AssetType Type { get; }

        public abstract Image GetImage();

        public virtual Stream Open()
        {
            return File.Exists(InstallationPath) ? File.OpenRead(InstallationPath) : null;
        }

        #endregion

        #region Equality members

        protected bool Equals(Asset other)
        {
            return string.Equals(InstallationPath, other.InstallationPath) && Type == other.Type;
        }

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        ///     true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Asset) obj);
        }

        /// <summary>
        ///     Serves as the default hash function.
        /// </summary>
        /// <returns>
        ///     A hash code for the current object.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((InstallationPath?.GetHashCode() ?? 0)*397) ^ (int) Type;
            }
        }

        #endregion
    }
}