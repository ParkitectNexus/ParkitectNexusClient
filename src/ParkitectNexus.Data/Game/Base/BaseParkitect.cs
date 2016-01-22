// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.AssetMagic.Readers;
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
        protected ILogger Logger { get; }
        protected ISettingsRepositoryFactory Factory { get; }

        protected BaseParkitect(ISettingsRepositoryFactory settingsRepositoryFactory, ILogger logger)
        {
            Logger = logger;
            Factory = settingsRepositoryFactory;
        }

        /// <summary>
        ///     Gets a collection of enabled and development mods.
        /// </summary>
        public virtual IEnumerable<IParkitectMod> ActiveMods
            => InstalledMods.Where(mod => mod.IsEnabled || mod.IsDevelopment);

        /// <summary>
        ///     Gets or sets the installation path.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the value is invalid.</exception>
        public virtual string InstallationPath
        {
            get
            {
                var gameSettings = Factory.Repository<GameSettings>();
                return IsValidInstallationPath(gameSettings.Model.InstallationPath)
                    ? gameSettings.Model.InstallationPath
                    : null;
            }
            set
            {
                var gameSettings = Factory.Repository<GameSettings>();
                if (!IsValidInstallationPath(value))
                    throw new ArgumentException("invalid installation path", nameof(value));

                gameSettings.Model.InstallationPath = value;
                gameSettings.Save();
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
        ///     Gets a collection of assembly names provided by the game.
        /// </summary>
        public virtual IEnumerable<string> ManagedAssemblyNames
            =>
                !IsInstalled
                    ? null
                    : Directory.GetFiles(Paths.DataManaged, "*.dll").Select(Path.GetFileName);

        /// <summary>
        ///     Gets a collection of installed mods.
        /// </summary>
        public virtual IEnumerable<IParkitectMod> InstalledMods
        {
            get
            {
                if (!IsInstalled)
                    yield break;

                // Iterate trough every directory in the mods directory which has a mod.json file.
                foreach (
                    var path in
                        Directory.GetDirectories(Paths.Mods).Where(path => File.Exists(Path.Combine(path, "mod.json"))))
                {
                    // Attempt to deserialize the mod.json file.
                    var mod = new ParkitectMod(this, Logger);
                    try
                    {
                        JsonConvert.PopulateObject(File.ReadAllText(Path.Combine(path, "mod.json")), mod);
                        mod.InstallationPath = path;
                    }
                    catch
                    {
                        mod = null;
                    }

                    // If the mod.json file was deserialized successfully, return the mod.
                    if (mod != null)
                        yield return mod;
                }
            }
        }

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

        /// <summary>
        ///     Stores the specified asset in the game's correct directory.
        /// </summary>
        /// <param name="downloadedAsset">The asset.</param>
        /// <returns>A task which performs the requested action.</returns>
        public virtual async Task StoreAsset(IParkitectDownloadedAsset downloadedAsset)
        {
            if (downloadedAsset == null) throw new ArgumentNullException(nameof(downloadedAsset));

            if (!IsInstalled)
                throw new Exception("parkitect is not installed");

            throw new NotImplementedException();
        }

        /// <summary>
        ///     Gets the installed assets of the specified type.
        /// </summary>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns>A collection of installed assets.</returns>
        public virtual IEnumerable<IParkitectAsset> GetAssets(ParkitectAssetType assetType)
        {

            var dataCacheFactory = ObjectFactory.GetInstance<IParkitectAssetDataCacheFactory>();
            var info = assetType.GetCustomAttribute<ParkitectAssetInfoAttribute>();
            switch (assetType)
            {
                case ParkitectAssetType.Mod:
                    foreach (var mod in InstalledMods)
                        yield return mod;
                    break;
                case ParkitectAssetType.Blueprint:
                {
                    var dataCache = dataCacheFactory.GetBlueprintCache();
                    var storagePath = Paths.GetPathInSavesFolder(info.StorageFolder);

                    var fileNames = new List<string>();
                    foreach (var path in Directory.GetFiles(storagePath, "*.png", SearchOption.AllDirectories))
                    {
                        var fileName = PathUtility.MakeRelativePath(storagePath, path);
                        fileNames.Add(fileName);

                        var data = dataCache.GetCachedData(fileName, ResolveBlueprintData);
                        yield return new ParkitectAsset(data.Name, path, data.Thumbnail, ParkitectAssetType.Blueprint);
                    }

                    dataCache.ClearCachedData(fileNames);
                    dataCache.Save();

                    break;
                }
                case ParkitectAssetType.Savegame:
                {
                    var dataCache = dataCacheFactory.GetSavegameCache();
                    var storagePath = Paths.GetPathInSavesFolder(info.StorageFolder);

                    var fileNames = new List<string>();
                    foreach (var path in Directory.GetFiles(storagePath, "*.txt", SearchOption.AllDirectories))
                    {
                        var fileName = PathUtility.MakeRelativePath(storagePath, path);
                        fileNames.Add(fileName);

                        var data = dataCache.GetCachedData(fileName, ResolveSavegameData);
                        yield return new ParkitectAsset(data.Name, path, data.Thumbnail, ParkitectAssetType.Blueprint);
                    }

                    dataCache.ClearCachedData(fileNames);
                    dataCache.Save();

                    break;
                }
            }
        }

        public virtual int GetAssetCount(ParkitectAssetType assetType)
        {
            switch (assetType)
            {
                case ParkitectAssetType.Blueprint:
                    return Directory.GetFiles(Paths.Blueprints, "*.png", SearchOption.AllDirectories).Length;
                case ParkitectAssetType.Mod:
                    return InstalledMods.Count();
                case ParkitectAssetType.Savegame:
                    return Directory.GetFiles(Paths.Savegames, "*.txt", SearchOption.AllDirectories).Length;
                default:
                    throw new Exception("unsupported asset type");
            }
        }

        private AssetCacheData ResolveBlueprintData(string fileName)
        {
            try
            {
                using (var im = (Bitmap) Image.FromFile(Path.Combine(Paths.Blueprints, fileName)))
                {
                    var reader = new BlueprintReader();
                    var data = reader.Read(im);
                    return new AssetCacheData {Name = data?.Header.Name, Thumbnail = new Bitmap(im, 100, 100)};
                }
            }
            catch
            {
                return null;
            }
        }

        private AssetCacheData ResolveSavegameData(string fileName)
        {
            try
            {
                var reader = new SavegameReader();
                var data = reader.Deserialize(File.ReadAllText(Path.Combine(Paths.Savegames, fileName)));
                return new AssetCacheData {Name = data?.Header.Name, Thumbnail = new Bitmap(data.Screenshot, 100, 100)};
            }
            catch
            {
                return null;
            }
        }

        protected abstract bool IsValidInstallationPath(string path);

        protected virtual void CompileActiveMods()
        {
            foreach (var mod in ActiveMods)
                mod.Compile();
        }
    }
}
