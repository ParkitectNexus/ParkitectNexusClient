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
        ///     Gets the API asset data.
        /// </summary>
        public ApiAsset ApiAsset { get; }

        /// <summary>
        ///     Gets the information.
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
        ///     Finalizes an instance of the <see cref="DownloadedAsset" /> class.
        /// </summary>
        ~DownloadedAsset()
        {
            Dispose(false);
        }

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