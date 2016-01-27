// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.API;

namespace ParkitectNexus.Data.Game
{
    /// <summary>
    ///     Provides the functionality of a Parkitect asset.
    /// </summary>
    public interface IDownloadedAsset : IDisposable
    {
        /// <summary>
        ///     Gets the name of the file.
        /// </summary>
        string FileName { get; }

        /// <summary>
        ///     Gets the API asset data.
        /// </summary>
        ApiAsset ApiAsset { get; }

        /// <summary>
        ///     Gets the information.
        /// </summary>
        DownloadInfo Info { get; }

        /// <summary>
        ///     Gets the stream.
        /// </summary>
        Stream Stream { get; }
    }
}
