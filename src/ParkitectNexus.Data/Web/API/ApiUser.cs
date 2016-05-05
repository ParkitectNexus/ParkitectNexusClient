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
    public class ApiUser : IApiResource
    {
        #region Implementation of IApiResource

        /// <summary>
        ///     Gets or sets the identifier of this resource.
        /// </summary>
        [JsonProperty("identifier")]
        public string Id { get; set; }

        #endregion

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
