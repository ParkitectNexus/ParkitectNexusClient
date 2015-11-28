// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

namespace ParkitectNexus.Data
{
    /// <summary>
    ///     Provides the functionality of a paths collection for the Parkitect game.
    /// </summary>
    public interface IParkitectPaths
    {
        string Data { get; }
        string DataManaged { get; }
        string Installation { get; }
        string Mods { get; }
        string NativeMods { get; }

        string GetPathInSavesFolder(string path);
        string GetPathInSavesFolder(string path, bool createIfNotExists);
        string GetPathInGameFolder(string path);
        string GetPathInGameFolder(string path, bool createIfNotExists);
    }
}