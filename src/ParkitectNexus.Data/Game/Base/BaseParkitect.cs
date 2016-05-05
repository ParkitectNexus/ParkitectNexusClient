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
using ParkitectNexus.Data.Settings;
using ParkitectNexus.Data.Settings.Models;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Game.Base
{
    /// <summary>
    ///     Represents the Parkitect game.
    /// </summary>
    public abstract class BaseParkitect : IParkitect
    {
        protected BaseParkitect(ISettingsRepository<GameSettings> gameSettingsRepository, ILogger logger)
        {
            Logger = logger;
            GameSettings = gameSettingsRepository;
            Assets = ObjectFactory.Container.With<IParkitect>(this).GetInstance<ILocalAssetRepository>();
        }

        protected ILogger Logger { get; }

        protected ISettingsRepository<GameSettings> GameSettings { get; }

        /// <summary>
        ///     Gets the installation path.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the value is invalid.</exception>
        public virtual string InstallationPath
        {
            get
            {
                return IsValidInstallationPath(GameSettings.Model.InstallationPath)
                    ? GameSettings.Model.InstallationPath
                    : null;
            }
            protected set
            {
                if (!IsValidInstallationPath(value))
                    throw new ArgumentException("invalid installation path", nameof(value));

                GameSettings.Model.InstallationPath = value;
                GameSettings.Save();
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the game is installed.
        /// </summary>
        public virtual bool IsInstalled => IsValidInstallationPath(InstallationPath);

        /// <summary>
        ///     Gets a collection of paths.
        /// </summary>
        public abstract IParkitectPaths Paths { get; }

        /// <summary>
        ///     Gets the assets repository.
        /// </summary>
        public ILocalAssetRepository Assets { get; }

        /// <summary>
        ///     Sets the installation path if the specified path is a valid installation path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>true if valid; false otherwise.</returns>
        public virtual bool SetInstallationPathIfValid(string path)
        {
            if (IsValidInstallationPath(path))
            {
                InstallationPath = path;
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Detects the installation path.
        /// </summary>
        /// <returns>true if the installation path has been detected; false otherwise.</returns>
        public abstract bool DetectInstallationPath();

        /// <summary>
        ///     Launches the game.
        /// </summary>
        public abstract void Launch();

        protected abstract bool IsValidInstallationPath(string path);
    }
}