// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using Newtonsoft.Json;

namespace ParkitectNexus.Data.Web.API
{
    /// <summary>
    ///     Represents a mod resource.
    /// </summary>
    [JsonObject]
    public class ApiModResource : IApiResource
    {
        /// <summary>
        ///     Gets or sets the GitHub source repository of this mod.
        /// </summary>
        [JsonProperty("source")]
        public string Source { get; set; }

        /// <summary>
        ///     Gets or sets the identifier of this resource.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}