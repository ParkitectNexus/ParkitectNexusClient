// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using Newtonsoft.Json;

namespace ParkitectNexus.Data.Web.API
{
    /// <summary>
    ///     Represents a mix of <see cref="ApiBlueprintResource" /> and <see cref="ApiModResource" />.
    /// </summary>
    [JsonObject]
    public class ApiMixedResource : ApiBlueprintResource
    {
        /// <summary>
        ///     Gets or sets the GitHub source repository of this mod.
        /// </summary>
        [JsonProperty("source")]
        public string Source { get; set; }
    }
}
