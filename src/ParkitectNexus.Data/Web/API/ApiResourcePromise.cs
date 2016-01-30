// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
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
        private readonly IParkitectNexusWebClientFactory _webClientFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiResourcePromise{T}" /> class.
        /// </summary>
        public ApiResourcePromise()
        {
            _webClientFactory = ObjectFactory.GetInstance<IParkitectNexusWebClientFactory>();
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
