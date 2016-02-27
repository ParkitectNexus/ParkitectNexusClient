// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

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

        #region Overrides of Object

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return $"{Type}Asset, Name: {Name}";
        }

        #endregion

        #region Equality members

        protected bool Equals(Asset other)
        {
            return string.Equals(InstallationPath, other.InstallationPath) && Type == other.Type;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Asset) obj);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>
        /// A hash code for the current object.
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
