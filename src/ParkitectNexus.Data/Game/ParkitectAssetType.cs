// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

namespace ParkitectNexus.Data.Game
{
    /// <summary>
    ///     Contains parkitect asset types supported by the client.
    /// </summary>
    public enum ParkitectAssetType
    {
        /// <summary>
        ///     A savegame.
        /// </summary>
        [ParkitectAssetInfo("text/plain", "Savegame", "Saves\\Savegames", ParkitectAssetStorageType.File)] Savegame,

        /// <summary>
        ///     A blueprint.
        /// </summary>
        [ParkitectAssetInfo("image/png", "Blueprint", "Saves\\Blueprints", ParkitectAssetStorageType.File)] Blueprint,

        /// <summary>
        ///     A mod.
        /// </summary>
        [ParkitectAssetInfo("application/zip", "Mod", "pnmods", ParkitectAssetStorageType.Folder)] Mod
    }
}