// ParkitectNexusInstaller
// Copyright 2015 Parkitect, Tim Potze

namespace ParkitectNexusClient
{
    /// <summary>
    /// Contains parkitect asset types supported by the client.
    /// </summary>
    internal enum ParkitectAssetType
    {
        /// <summary>
        /// A savegame.
        /// </summary>
        [ParkitectAssetInfo("text/plain", "Savegame", "Saves\\Savegames")] Savegame,
        /// <summary>
        /// A blueprint.
        /// </summary>
        [ParkitectAssetInfo("image/png", "Blueprint", "Saves\\Blueprints")] Blueprint
    }
}