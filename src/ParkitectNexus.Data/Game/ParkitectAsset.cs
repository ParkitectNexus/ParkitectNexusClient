// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using ParkitectNexus.Data.Web;

namespace ParkitectNexus.Data.Game
{
    /// <summary>
    ///     Represents a Parkitect asset.
    /// </summary>
    public class ParkitectAsset : IParkitectAsset
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ParkitectAsset" /> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="downloadInfo">The download info.</param>
        /// <param name="type">The type.</param>
        /// <param name="stream">The stream.</param>
        /// <exception cref="ArgumentNullException">Thrown if fileName or stream is null.</exception>
        public ParkitectAsset(string fileName, DownloadInfo downloadInfo, ParkitectAssetType type, Stream stream)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            FileName = fileName;
            DownloadInfo = downloadInfo;
            Stream = stream;
            Type = type;
        }

        /// <summary>
        ///     Gets the name of the file.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        ///     Gets the download info of the file.
        /// </summary>
        public DownloadInfo DownloadInfo { get; }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        public ParkitectAssetType Type { get; }

        /// <summary>
        ///     Gets the stream.
        /// </summary>
        public Stream Stream { get; }

        #region Implementation of IDisposable

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        ///     Finalizes an instance of the <see cref="ParkitectAsset" /> class.
        /// </summary>
        ~ParkitectAsset()
        {
            Dispose(false);
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
            return $"{{{FileName} {Type}}}";
        }

        #endregion

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stream?.Dispose();
            }
        }
    }
}