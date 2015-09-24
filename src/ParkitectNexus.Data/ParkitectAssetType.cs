// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

namespace ParkitectNexus.Data
{
    /// <summary>
    ///     Contains parkitect asset types supported by the client.
    /// </summary>
    public enum ParkitectAssetType
    {
        /// <summary>
        ///     A savegame.
        /// </summary>
        [ParkitectAssetInfo("text/plain", "Savegame", "Saves\\Savegames")] Savegame,

        /// <summary>
        ///     A blueprint.
        /// </summary>
        [ParkitectAssetInfo("image/png", "Blueprint", "Saves\\Blueprints")]
        Blueprint,
        /// <summary>
        ///     A mod.
        /// </summary>
        [ParkitectAssetInfo("application/zip", "Mod", "mods")]
        Mod
    }
}