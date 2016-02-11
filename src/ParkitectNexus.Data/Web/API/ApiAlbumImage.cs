// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Drawing;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.Data.Web.Client;

namespace ParkitectNexus.Data.Web.API
{
    /// <summary>
    ///     Represents an image in an album.
    /// </summary>
    [JsonObject]
    public class ApiAlbumImage
    {
        private readonly INexusWebClientFactory _webClientFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiAlbumImage" /> class.
        /// </summary>
        public ApiAlbumImage()
        {
            _webClientFactory = ObjectFactory.GetInstance<INexusWebClientFactory>();
        }

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the URL.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        ///     Gets or sets the name of the file.
        /// </summary>
        [JsonProperty("filename")]
        public string FileName { get; set; }

        /// <summary>
        ///     Gets the image this instance represents.
        /// </summary>
        public async Task<Image> Get()
        {
            using (var webClient = _webClientFactory.CreateWebClient(true))
                return Image.FromStream(await webClient.OpenReadTaskAsync(Url));
        }
    }
}