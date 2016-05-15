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
        private readonly ILogger _log;
        private readonly INexusWebClientFactory _webClientFactory;
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
                var url = _website.ResolveUrl("api/assets/" + id + "?include=image,user,resource,dependencies");

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
        ///     Gets the identifiers of the required mods.
        /// </summary>
        /// <returns>The identifiers of the required mods.</returns>
        public async Task<string[]> GetRequiredModIdentifiers()
        {
            using (var webClient = _webClientFactory.CreateWebClient())
            using (var stream = await webClient.OpenReadTaskAsync(_website.ResolveUrl("api/assets/required", "client")))
            using (var streamReader = new StreamReader(stream))
            {
                var info = JsonConvert.DeserializeObject<ApiDataContainer<string[]>>(await streamReader.ReadToEndAsync());
                return info.Data;
            }
        }

        /// <summary>
        ///     Gets the subscriptions of the authenticated user.
        /// </summary>
        /// <param name="authKey">The authentication key.</param>
        /// <returns>
        ///     The subscriptions.
        /// </returns>
        public Task<ApiSubscription[]> GetSubscriptions(string authKey)
        {
            throw new NotImplementedException();
//            var url = _website.ResolveUrl("api/subscriptions");
//
//            using (var client = _webClientFactory.CreateWebClient(true))
//            {
//                _log.WriteLine($"Fetching subscriptions for auth key {authKey.Substring(0, 4)}xxxxxxxxxx.");
//
//                using (var stream = client.OpenRead(url))
//                using (var reader = new StreamReader(stream))
//                    return
//                        JsonConvert.DeserializeObject<ApiDataContainer<ApiSubscription[]>>(await reader.ReadToEndAsync())
//                            .Data;
//            }
        }

        /// <summary>
        ///     Gets user info of the authenticated user.
        /// </summary>
        /// <param name="authKey">The authentication key.</param>
        /// <returns>The user information.</returns>
        public Task<ApiUser> GetUserInfo(string authKey)
        {
            throw new NotImplementedException();
//            var url = _website.ResolveUrl("api/users/me");
//
//            using (var client = _webClientFactory.CreateWebClient(true))
//            {
//                _log.WriteLine($"Fetching user info for auth key {authKey.Substring(0, 4)}xxxxxxxxxx.");
//
//                using (var stream = client.OpenRead(url))
//                using (var reader = new StreamReader(stream))
//                    return
//                        JsonConvert.DeserializeObject<ApiDataContainer<ApiUser>>(await reader.ReadToEndAsync())
//                            .Data;
//            }
        }
    }
}
