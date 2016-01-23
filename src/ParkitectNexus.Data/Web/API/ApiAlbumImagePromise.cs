// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using Newtonsoft.Json;
using ParkitectNexus.Data.Web.Client;

namespace ParkitectNexus.Data.Web.API
{
    /// <summary>
    ///     Reprents a promise of an image in an album.
    /// </summary>
    [JsonObject]
    public class ApiAlbumImagePromise : ApiResourcePromise<ApiAlbumImage>
    {
        /// <summary>
        ///     Gets or sets the URL.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
