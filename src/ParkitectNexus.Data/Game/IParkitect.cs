// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using ParkitectNexus.Data.Assets;

namespace ParkitectNexus.Data.Game
{
    /// <summary>
    ///     Provides the functionality of a Parkitect game.
    /// </summary>
    public interface IParkitect
    {
        /// <summary>
        ///     Gets the installation path.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the installation path is invalid</exception>
        string InstallationPath { get; }

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
        ///     Launches the game.
        /// </summary>
        void Launch();

        /// <summary>
        ///     Sets the installation path if the specified path is a valid installation path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>true if valid; false otherwise.</returns>
        bool SetInstallationPathIfValid(string path);
    }
}