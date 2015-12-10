// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.IO;
using ParkitectNexus.Data.Web;

namespace ParkitectNexus.Data.Game
{
    /// <summary>
    ///     Provides the functionality of a Parkitect asset.
    /// </summary>
    public interface IParkitectAsset : IDisposable
    {
        /// <summary>
        ///     Gets the name of the file.
        /// </summary>
        string FileName { get; }

        /// <summary>
        ///     Gets the download info of the file.
        /// </summary>
        DownloadInfo DownloadInfo { get; }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        ParkitectAssetType Type { get; }

        /// <summary>
        ///     Gets the stream.
        /// </summary>
        Stream Stream { get; }
    }
}