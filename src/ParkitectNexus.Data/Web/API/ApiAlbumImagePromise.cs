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