// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Data.Game
{
    /// <summary>
    ///     Provides information about a parkitect asset.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ParkitectAssetInfoAttribute : Attribute
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParkitectAssetInfoAttribute" /> class.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="name">The name.</param>
        /// <param name="storageFolder">The storage folder.</param>
        /// <param name="storageType">The storage type.</param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public ParkitectAssetInfoAttribute(string contentType, string name, string storageFolder, ParkitectAssetStorageType storageType)
        {
            StorageType = storageType;
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (storageFolder == null) throw new ArgumentNullException(nameof(storageFolder));
            ContentType = contentType;
            Name = name;
            StorageFolder = storageFolder;
        }

        /// <summary>
        ///     Gets the type of the content provided when an asset of this type is being downloaded.
        /// </summary>
        public string ContentType { get; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the storage folder within the game directory.
        /// </summary>
        public string StorageFolder { get; }

        /// <summary>
        ///     Gets the storage type of the asset.
        /// </summary>
        public ParkitectAssetStorageType StorageType { get; }
    }
}