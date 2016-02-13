// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using Newtonsoft.Json;

namespace ParkitectNexus.Data.Assets.Meta
{
    [JsonObject]
    public class ModMetadata : AssetMetadata
    {
        public string Tag { get; set; }

        public string Repository { get; set; }
    }
}
