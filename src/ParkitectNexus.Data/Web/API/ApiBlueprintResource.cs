// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using Newtonsoft.Json;

namespace ParkitectNexus.Data.Web.API
{
    /// <summary>
    ///     Represents a blueprint resource.
    /// </summary>
    [JsonObject]
    public class ApiBlueprintResource : IApiResource
    {
        /// <summary>
        ///     Gets or sets the name of the file of this blueprint.
        /// </summary>
        [JsonProperty("filename")]
        public string FileName { get; set; }

        /// <summary>
        ///     Gets or sets the asset resource promise associated with this blueprint resource.
        /// </summary>
        [JsonProperty("asset")]
        public ApiResourcePromise<ApiAsset> Asset { get; set; }

        /// <summary>
        ///     Gets or sets the identifier of this resource.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}