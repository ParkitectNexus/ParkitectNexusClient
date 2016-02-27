// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

namespace ParkitectNexus.Data.Assets.Meta
{
    public interface IModMetadata : IAssetMetadata
    {
        string Tag { get; set; }

        string Repository { get; set; }
    }
}
