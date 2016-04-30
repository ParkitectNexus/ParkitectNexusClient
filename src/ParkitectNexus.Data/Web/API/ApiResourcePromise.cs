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

using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.Data.Web.Client;

namespace ParkitectNexus.Data.Web.API
{
    /// <summary>
    ///     Represents a promise of a resource of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of resource promised</typeparam>
    [JsonObject]
    public class ApiResourcePromise<T> where T : class
    {
        private readonly INexusWebClientFactory _webClientFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiResourcePromise{T}" /> class.
        /// </summary>
        public ApiResourcePromise()
        {
            _webClientFactory = ObjectFactory.GetInstance<INexusWebClientFactory>();
        }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        ///     Gets or sets the API URL.
        /// </summary>
        [JsonProperty("api_url")]
        public string ApiUrl { get; set; }

        /// <summary>
        ///     Gets the resource.
        /// </summary>
        /// <returns>The resource</returns>
        public async Task<T> GetResource()
        {
            if (ApiUrl == null)
                return null;

            using (var client = _webClientFactory.CreateWebClient())
            using (var stream = client.OpenRead(ApiUrl))
            using (var reader = new StreamReader(stream))
                return
                    JsonConvert.DeserializeObject<ApiDataContainer<T>>(await reader.ReadToEndAsync()).Data;
        }
    }

    public class ApiResourcePromiseWithUrl<T> : ApiResourcePromise<T> where T : class
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
