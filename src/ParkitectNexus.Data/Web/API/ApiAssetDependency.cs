// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using Newtonsoft.Json;

namespace ParkitectNexus.Data.Web.API
{
    /// <summary>
    ///
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiAssetDependency
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="ApiAssetDependency"/> is required.
        /// </summary>
        [JsonProperty("required")]
        public bool Required { get; set; }

        /// <summary>
        ///     Gets or sets the asset.
        /// </summary>
        [JsonProperty("asset")]
        public ApiResourcePromiseWithUrl<ApiAsset> Asset { get; set; }
    }
}
