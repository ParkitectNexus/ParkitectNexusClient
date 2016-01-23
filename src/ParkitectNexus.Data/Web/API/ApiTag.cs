// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using Newtonsoft.Json;

namespace ParkitectNexus.Data.Web.API
{
    /// <summary>
    ///     Represents an asset tag.
    /// </summary>
    [JsonObject]
    public class ApiTag
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        [JsonProperty("tag")]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the slug.
        /// </summary>
        [JsonProperty("slug")]
        public string Slug { get; set; }

        /// <summary>
        ///     Gets or sets the type.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        ///     Gets or sets the count.
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
