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
using ParkitectNexus.Data.Assets;

namespace ParkitectNexus.Data.Web.API
{
    /// <summary>
    ///     Represents an API asset.
    /// </summary>
    public class ApiAsset : IApiObject
    {
        #region Implementation of IApiObject

        /// <summary>
        ///     Gets or sets the identifier of this resource.
        /// </summary>
        [JsonProperty("identifier")]
        public string Id { get; set; }

        #endregion

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("image")]
        public ApiDataContainer<ApiImage> Image { get; set; }

        [JsonProperty("user")]
        public ApiDataContainer<ApiUser> User { get; set; }

        [JsonProperty("dependencies")]
        public ApiDataContainer<ApiAsset[]> Dependencies { get; set; }

        [JsonProperty("type")]
        public string TypeString { get; set; }

        [JsonProperty("resource")]
        public ApiDataContainer<ApiResourceSource> Resource { get; set; }

        [JsonIgnore]
        public AssetType Type => AssetTypeUtil.Parse(TypeString);
    }
}
