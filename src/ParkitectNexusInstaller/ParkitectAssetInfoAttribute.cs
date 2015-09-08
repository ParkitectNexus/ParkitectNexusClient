// ParkitectNexusInstaller
// Copyright 2015 Parkitect, Tim Potze

using System;

namespace ParkitectNexusInstaller
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class ParkitectAssetInfoAttribute : Attribute
    {
        public ParkitectAssetInfoAttribute(string contentType, string name, string storageFolder)
        {
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (storageFolder == null) throw new ArgumentNullException(nameof(storageFolder));
            ContentType = contentType;
            Name = name;
            StorageFolder = storageFolder;
        }

        public string ContentType { get; }
        public string Name { get; }
        public string StorageFolder { get; }
    }
}