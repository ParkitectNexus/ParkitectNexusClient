// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using Newtonsoft.Json;

namespace ParkitectNexus.Data.Web.API
{
    /// <summary>
    ///     Represents a json data container.
    /// </summary>
    /// <typeparam name="T">The data type.</typeparam>
    [JsonObject]
    public class ApiDataContainer<T>
    {
        /// <summary>
        ///     Gets or sets the data.
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
