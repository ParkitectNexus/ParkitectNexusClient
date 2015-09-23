// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ParkitectNexus.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ParkitectMod
    {
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public string Version { get; set; }
        [JsonProperty]
        public string FolderName { get; set; }
        [JsonProperty]
        public string NameSpace { get; set; }
        [JsonProperty]
        public string ClassName { get; set; }
        [JsonProperty]
        public bool ForceCompile { get; set; }
        [JsonProperty]
        public IList<string> CodeFiles { get; set; } = new List<string>();

        public string AssemblyFileName => $"{NameSpace}.dll";
    }
}