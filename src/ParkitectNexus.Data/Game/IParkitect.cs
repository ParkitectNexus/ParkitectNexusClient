// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Diagnostics;
using ParkitectNexus.Data.Assets;

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
        ///     Gets a value indicating whether the game is installed.
        /// </summary>
        bool IsInstalled { get; }

        /// <summary>
        ///     Gets a collection of paths.
        /// </summary>
        IParkitectPaths Paths { get; }

        /// <summary>
        ///     Gets the assets repository.
        /// </summary>
        ILocalAssetRepository Assets { get; }

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
    }
}
