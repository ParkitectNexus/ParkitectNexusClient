// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

namespace ParkitectNexus.Data
{
    public interface IPathResolver
    {
        string AppData();

        bool IsParkitectInstalled();
        string ParkitectPath();
    }
}