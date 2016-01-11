// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

namespace ParkitectNexus.Data.Utilities
{
    public interface IOperatingSystem
    {
        SupportedOperatingSystem Detect();
    }
}
