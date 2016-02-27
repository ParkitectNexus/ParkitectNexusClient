// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web.Client;

namespace ParkitectNexus.Data.Web.API
{
    /// <summary>
    ///     Represents the ParkitectNexus API.
    /// </summary>
    public class ParkitectNexusAPI : IParkitectNexusAPI
    {
        private readonly INexusWebClientFactory _webClientFactory;
        private readonly ILogger _log;
        private readonly IWebsite _website;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParkitectNexusAPI" /> class.
        /// </summary>
        /// <param name="website">The website.</param>
        /// <param name="webClientFactory">The web client factory.</param>
        /// <param name="log"></param>
        /// <exception cref="ArgumentNullException">website or webClientFactory is null.</exception>
        public ParkitectNexusAPI(IWebsite website, INexusWebClientFactory webClientFactory, ILogger log)
        {
            _website = website;
            _webClientFactory = webClientFactory;
            _log = log;
        }

        /// <summary>
        ///     Gets the asset with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///     The asset.
        /// </returns>
        public async Task<ApiAsset> GetAsset(string id)
        {
            try
            {
                _log.WriteLine($"Fetching information from the PN API for asset {id}.");
                var url = _website.ResolveUrl("api/assets/" + id);

                using (var client = _webClientFactory.CreateWebClient())
                using (var stream = client.OpenRead(url))
                using (var reader = new StreamReader(stream))
                {
                    var data = await reader.ReadToEndAsync();
                    var deserialized = JsonConvert.DeserializeObject<ApiDataContainer<ApiAsset>>(data);

                    return deserialized.Data;
                }
            }
            catch (Exception e)
            {
                _log.WriteLine($"Failed to fetch information for asset {id}!", LogLevel.Error);
                _log.WriteException(e);

                return null;
            }
        }

        /// <summary>
        ///     Gets the subscriptions of the authenticated user.
        /// </summary>
        /// <param name="authKey">The authentication key.</param>
        /// <returns>
        ///     The subscriptions.
        /// </returns>
        public async Task<ApiSubscription[]> GetSubscriptions(string authKey)
        {
            var url = _website.ResolveUrl("api/subscriptions");

            using (var client = _webClientFactory.CreateWebClient(true))
            {
                _log.WriteLine($"Fetching subscriptions for auth key {authKey.Substring(0, 4)}xxxxxxxxxx.");

                using (var stream = client.OpenRead(url))
                using (var reader = new StreamReader(stream))
                    return
                        JsonConvert.DeserializeObject<ApiDataContainer<ApiSubscription[]>>(await reader.ReadToEndAsync())
                            .Data;
            }
        }

        /// <summary>
        ///     Gets user info of the authenticated user.
        /// </summary>
        /// <param name="authKey">The authentication key.</param>
        /// <returns>The user information.</returns>
        public async Task<ApiUser> GetUserInfo(string authKey)
        {
            var url = _website.ResolveUrl("api/users/me");

            using (var client = _webClientFactory.CreateWebClient(true))
            {
                _log.WriteLine($"Fetching user info for auth key {authKey.Substring(0, 4)}xxxxxxxxxx.");

                using (var stream = client.OpenRead(url))
                using (var reader = new StreamReader(stream))
                    return
                        JsonConvert.DeserializeObject<ApiDataContainer<ApiUser>>(await reader.ReadToEndAsync())
                            .Data;
            }
        }
    }
}
