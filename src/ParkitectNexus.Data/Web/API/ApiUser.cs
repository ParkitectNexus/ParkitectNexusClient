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

using System.Drawing;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.Data.Web.Client;

namespace ParkitectNexus.Data.Web.API
{
    public class ApiUser
    {
        [JsonProperty("identifier")]
        public string Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("avatar")]
        public string AvatarUrl { get; set; }

        [JsonProperty("steam")]
        public string SteamId { get; set; }

        [JsonProperty("twitch")]
        public string TwitchUsername { get; set; }

        [JsonProperty("twitter")]
        public string TwitterUsername { get; set; }

        [JsonProperty("bitcoin")]
        public string BitcoinId { get; set; }

        [JsonProperty("paypal")]
        public string PaypalMeUsername { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        public async Task<Image> GetAvatar()
        {
            if (AvatarUrl == null)
                return null;

            var webClientFactory = ObjectFactory.GetInstance<INexusWebClientFactory>();

            using (var client = webClientFactory.CreateWebClient())
            using (var stream = await client.OpenReadTaskAsync(AvatarUrl))
                return Image.FromStream(stream);
        }
    }
}