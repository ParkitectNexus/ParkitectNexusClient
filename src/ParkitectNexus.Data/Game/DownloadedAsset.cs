// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Web.API;

namespace ParkitectNexus.Data.Game
{
    /// <summary>
    ///     Represents a Parkitect asset.
    /// </summary>
    public class DownloadedAsset : IDownloadedAsset
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DownloadedAsset" /> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="apiAsset"></param>
        /// <param name="info"></param>
        /// <param name="stream">The stream.</param>
        /// <exception cref="ArgumentNullException">fileName or stream is null.</exception>
        public DownloadedAsset(string fileName, ApiAsset apiAsset, DownloadInfo info, Stream stream)
        {
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            if (apiAsset == null) throw new ArgumentNullException(nameof(apiAsset));
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            FileName = fileName;
            ApiAsset = apiAsset;
            Info = info;
            Stream = stream;
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="DownloadedAsset" /> class.
        /// </summary>
        ~DownloadedAsset()
        {
            Dispose(false);
        }

        /// <summary>
        ///     Gets the API asset data.
        /// </summary>
        public ApiAsset ApiAsset { get; }

        /// <summary>
        /// Gets the information.
        /// </summary>
        public DownloadInfo Info { get; }

        /// <summary>
        ///     Gets the name of the file.
        /// </summary>
        public string FileName { get; }

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
