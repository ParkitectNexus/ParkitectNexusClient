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
    public class ApiAlbumImagePromise : ApiResourcePromiseWithUrl<ApiAlbumImage>
    {
        private readonly INexusWebClientFactory _webClientFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApiAlbumImagePromise" /> class.
        /// </summary>
        public ApiAlbumImagePromise()
        {
            _webClientFactory = ObjectFactory.GetInstance<INexusWebClientFactory>();
        }

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