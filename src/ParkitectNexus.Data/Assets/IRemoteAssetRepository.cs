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
using System.Threading.Tasks;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web.API;

namespace ParkitectNexus.Data.Assets
{
    /// <summary>
    ///     Provides the functionality of an online Parkitect asset storage.
    /// </summary>
    public interface IRemoteAssetRepository
    {
        /// <summary>
        ///     Downloads the asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception">the specified asset id is invalid</exception>
        Task<IDownloadedAsset> DownloadAsset(ApiAsset asset);

        /// <summary>
        ///     Gets the latest mod tag.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <returns>The latest tag.</returns>
        Task<string> GetLatestModTag(IModAsset asset);
    }
}