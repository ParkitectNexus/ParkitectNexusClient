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
    ///     Represents the ParkitectNexus API.
    /// </summary>
    public class ParkitectNexusAPI : IParkitectNexusAPI
    {
        private readonly INexusWebClientFactory _webClientFactory;
        private readonly IWebsite _website;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParkitectNexusAPI" /> class.
        /// </summary>
        /// <param name="website">The website.</param>
        /// <param name="webClientFactory">The web client factory.</param>
        /// <exception cref="ArgumentNullException">website or webClientFactory is null.</exception>
        public ParkitectNexusAPI(IWebsite website, INexusWebClientFactory webClientFactory)
        {
            if (website == null) throw new ArgumentNullException(nameof(website));
            if (webClientFactory == null) throw new ArgumentNullException(nameof(webClientFactory));
            _website = website;
            _webClientFactory = webClientFactory;
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
            catch (Exception exception)
            {
                Console.WriteLine(exception);
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
                using (var stream = client.OpenRead(url))
                using (var reader = new StreamReader(stream))
                    return
                        JsonConvert.DeserializeObject<ApiDataContainer<ApiUser>>(await reader.ReadToEndAsync())
                            .Data;
            }
        }
    }
}