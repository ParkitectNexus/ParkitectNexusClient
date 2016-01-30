// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using Newtonsoft.Json;

namespace ParkitectNexus.Data.Web.API
{
    /// <summary>
    ///     Represents an image in an album.
    /// </summary>
    [JsonObject]
    public class ApiAlbumImage
    {
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
    }
}
