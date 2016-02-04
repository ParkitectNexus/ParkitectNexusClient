// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ParkitectNexus.Data.Assets
{
    /// <summary>
    ///     Contains parkitect asset types supported by the client.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AssetType
    {
        /// <summary>
        ///     A savegame.
        /// </summary>
        [AssetInfo("text/plain")] Park,

        /// <summary>
        ///     A blueprint.
        /// </summary>
        [AssetInfo("image/png")] Blueprint,

        /// <summary>
        ///     A mod.
        /// </summary>
        [AssetInfo("application/zip")] Mod
    }
}
