// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Game
{
    /// <summary>
    ///     Provides the functionality of a Parkitect game.
    /// </summary>
    public interface IParkitect
    {
        /// <summary>
        ///     Gets or sets the installation path.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the installation path is invalid</exception>
        string InstallationPath { get; set; }

        /// <summary>
        ///     Gets a collection of installed mods.
        /// </summary>
        IEnumerable<IParkitectMod> InstalledMods { get; }

        /// <summary>
        ///     Gets a value indicating whether the game is installed.
        /// </summary>
        bool IsInstalled { get; }

        /// <summary>
        ///     Gets a collection of assembly names provided by the game.
        /// </summary>
        IEnumerable<string> ManagedAssemblyNames { get; }

        /// <summary>
        ///     Gets a collection of paths.
        /// </summary>
        IParkitectPaths Paths { get; }

        /// <summary>
        ///     Detects the installation path.
        /// </summary>
        /// <returns>true if the installation path has been detected; false otherwise.</returns>
        bool DetectInstallationPath();

        /// <summary>
        ///     Launches the game with the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The launched process.</returns>
        Process Launch(string arguments = "-single-instance");

        /// <summary>
        ///     Sets the installation path if the specified path is a valid installation path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>true if valid; false otherwise.</returns>
        bool SetInstallationPathIfValid(string path);

        /// <summary>
        ///     Stores the specified asset in the game's correct directory.
        /// </summary>
        /// <param name="downloadedAsset">The asset.</param>
        /// <returns>A task which performs the requested action.</returns>
        Task StoreAsset(IParkitectDownloadedAsset downloadedAsset);

        /// <summary>
        ///     Gets the installed assets of the specified type.
        /// </summary>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns>A collection of installed assets.</returns>
        IEnumerable<IParkitectAsset> GetAssets(ParkitectAssetType assetType);

        /// <summary>
        ///     Gets the count of installed assets of the specified type.
        /// </summary>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns>The count of installed assets.</returns>
        int GetAssetCount(ParkitectAssetType assetType);
    }
}
