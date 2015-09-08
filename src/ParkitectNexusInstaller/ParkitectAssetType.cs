// ParkitectNexusInstaller
// Copyright 2015 Parkitect, Tim Potze

namespace ParkitectNexusInstaller
{
    internal enum ParkitectAssetType
    {
        [ParkitectAssetInfo("text/plain", "Savegame", "Saves\\Savegames")] Savegame,
        [ParkitectAssetInfo("image/png", "Blueprint", "Saves\\Blueprints")] Blueprint
    }
}