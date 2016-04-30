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