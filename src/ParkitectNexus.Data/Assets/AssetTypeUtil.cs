// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Data.Assets
{
    public static class AssetTypeUtil
    {
        public static AssetType Parse(string value)
        {
            if (value == null)
                return default(AssetType);

            AssetType parsed;
            if (Enum.TryParse(value, out parsed))
                return parsed;

            switch (value)
            {
                case "park":
                    return AssetType.Savegame;
                case "mod":
                    return AssetType.Mod;
                case "blueprint":
                    return AssetType.Blueprint;
                default:
                    return default(AssetType);
            }
        }
    }
}
