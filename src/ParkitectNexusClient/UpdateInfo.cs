// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using Newtonsoft.Json;

namespace ParkitectNexus.Client
{
    [JsonObject]
    internal class UpdateInfo
    {
        public string Version { get; set; }

        [JsonProperty("download_url")]
        public string DownloadUrl { get; set; }
    }
}