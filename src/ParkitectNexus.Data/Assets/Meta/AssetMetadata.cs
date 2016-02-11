// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using Newtonsoft.Json;

namespace ParkitectNexus.Data.Assets.Meta
{
    [JsonObject]
    public class AssetMetadata : IAssetMetadata
    {
        public string Id { get; set; }

        public DateTime InstalledVersion { get; set; }
    }
}
