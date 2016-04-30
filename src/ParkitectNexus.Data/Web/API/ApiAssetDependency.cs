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

using Newtonsoft.Json;

namespace ParkitectNexus.Data.Web.API
{
    /// <summary>
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiAssetDependency
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="ApiAssetDependency" /> is required.
        /// </summary>
        [JsonProperty("required")]
        public bool Required { get; set; }

        /// <summary>
        ///     Gets or sets the asset.
        /// </summary>
        [JsonProperty("asset")]
        public ApiResourcePromiseWithUrl<ApiAsset> Asset { get; set; }
    }
}