// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Data.Assets
{
    public class ModAsset : Asset
    {
        // todo: This should replace `ParkitectMod`

        public ModAsset(string path, AssetCachedData data) : base(path, data, AssetType.Mod)
        {
        }
    }
}
