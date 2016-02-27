// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using Newtonsoft.Json;

namespace ParkitectNexus.Data.Updating
{
    /// <summary>
    ///     Represents an application update.
    /// </summary>
    [JsonObject]
    public class UpdateInfo
    {
        /// <summary>
        ///     Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        ///     Gets or sets the download URL.
        /// </summary>
        [JsonProperty("download_url")]
        public string DownloadUrl { get; set; }
    }
}
