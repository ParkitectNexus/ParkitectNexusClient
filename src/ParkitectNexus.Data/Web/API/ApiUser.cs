// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

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

            var webClientFactory = ObjectFactory.GetInstance<IParkitectNexusWebClientFactory>();

            using (var client = webClientFactory.CreateWebClient())
            using (var stream = await client.OpenReadTaskAsync(AvatarUrl))
                return Image.FromStream(stream);
        }
    }
}
