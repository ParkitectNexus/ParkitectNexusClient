// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Data.Assets
{
    /// <summary>
    ///     Provides information about a parkitect asset.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class AssetInfoAttribute : Attribute
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="AssetInfoAttribute" /> class.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public AssetInfoAttribute(string contentType)
        {
            ContentType = contentType;
        }

        /// <summary>
        ///     Gets the type of the content provided when an asset of this type is being downloaded.
        /// </summary>
        public string ContentType { get; }
    }
}
