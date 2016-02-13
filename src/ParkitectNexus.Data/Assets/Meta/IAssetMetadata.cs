// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Data.Assets.Meta
{
    public interface IAssetMetadata
    {
        string Id { get; set; }

        DateTime InstalledVersion { get; set; }
    }
}