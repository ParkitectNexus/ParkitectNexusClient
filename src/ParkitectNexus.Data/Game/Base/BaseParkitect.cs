// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Diagnostics;
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
        ///     Gets or sets the installation path.
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
            set
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
        ///     Launches the game with the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The launched process.</returns>
        public abstract Process Launch(string arguments = "-single-instance");

        protected abstract bool IsValidInstallationPath(string path);
    }
}
