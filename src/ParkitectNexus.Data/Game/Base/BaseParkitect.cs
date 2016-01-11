// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.Data.Settings;
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

            Logger.WriteLine($"Storing asset {downloadedAsset}.");

            // Gather information about the asset type.
            var assetInfo = downloadedAsset.Type.GetCustomAttribute<ParkitectAssetInfoAttribute>();

            if (assetInfo == null)
                throw new Exception("invalid asset type");

            switch (downloadedAsset.Type)
            {
                case ParkitectAssetType.Blueprint:
                case ParkitectAssetType.Savegame:
                    // Create the directory where the asset should be stored and create a path to where the asset should be stored.
                    var storagePath =
                        Paths.GetPathInSavesFolder(assetInfo.StorageFolder.Replace('\\', Path.DirectorySeparatorChar));
                    var assetPath = Path.Combine(storagePath, downloadedAsset.FileName);

                    Directory.CreateDirectory(storagePath);

                    Logger.WriteLine($"Storing asset to {assetPath}.");

                    // If the file already exists, add a number behind the file name.
                    if (File.Exists(assetPath))
                    {
                        Logger.WriteLine("Asset already exists, comparing hashes.");

                        var md5 = MD5.Create();

                        // Compute hash of downloaded asset to match with installed hash.
                        downloadedAsset.Stream.Seek(0, SeekOrigin.Begin);
                        var validHash = md5.ComputeHash(downloadedAsset.Stream);

                        if (validHash.SequenceEqual(md5.ComputeHash(File.OpenRead(assetPath))))
                        {
                            Logger.WriteLine("Asset hashes match, aborting.");
                            return;
                        }

                        Logger.WriteLine("Asset hashes mismatch, computing new file name.");
                        // Separate the filename and the extension.
                        var attempt = 1;
                        var fileName = Path.GetFileNameWithoutExtension(downloadedAsset.FileName);
                        var fileExtension = Path.GetExtension(downloadedAsset.FileName);

                        // Update the path to where the the asset should be stored by adding a number behind the name until an available filename has been found.
                        do
                        {
                            assetPath = Path.Combine(storagePath, $"{fileName} ({++attempt}){fileExtension}");

                            if (File.Exists(assetPath) &&
                                validHash.SequenceEqual(md5.ComputeHash(File.OpenRead(assetPath))))
                                return;
                        } while (File.Exists(assetPath));

                        Logger.WriteLine($"Newly computed path is {assetPath}.");
                    }

                    Logger.WriteLine("Writing asset to file.");
                    // Write the stream to a file at the asset path.
                    using (var fileStream = File.Create(assetPath))
                    {
                        downloadedAsset.Stream.Seek(0, SeekOrigin.Begin);
                        await downloadedAsset.Stream.CopyToAsync(fileStream);
                    }
                    break;
                case ParkitectAssetType.Mod:

                    Logger.WriteLine("Attempting to open mod stream.");
                    using (var zip = new ZipArchive(downloadedAsset.Stream, ZipArchiveMode.Read))
                    {
                        // Compute name of main directory inside archive.
                        var mainFolder = zip.Entries.FirstOrDefault()?.FullName;
                        if (mainFolder == null)
                            throw new Exception("invalid archive");

                        Logger.WriteLine($"Mod archive main folder is {mainFolder}.");

                        // Find the mod.json file. Yay for / \ path divider compatibility.
                        var modJsonPath = Path.Combine(mainFolder, "mod.json").Replace('/', '\\');
                        var modJson = zip.Entries.FirstOrDefault(e => e.FullName.Replace('/', '\\') == modJsonPath);

                        // Read mod.json.
                        if (modJson == null) throw new Exception("mod is missing mod.json file");
                        using (var streamReader = new StreamReader(modJson.Open()))
                        {
                            var json = await streamReader.ReadToEndAsync();
                            var mod = new ParkitectMod(this, Logger);
                            JsonConvert.PopulateObject(json, mod);

                            Logger.WriteLine($"mod.json was deserialized to mod object '{mod}'.");

                            // Set default mod properties.
                            mod.Tag = downloadedAsset.DownloadInfo.Tag;
                            mod.Repository = downloadedAsset.DownloadInfo.Repository;
                            mod.InstallationPath = Path.Combine(Paths.Mods,
                                downloadedAsset.DownloadInfo.Repository.Replace('/', '@'));
                            mod.IsEnabled = true;
                            mod.IsDevelopment = false;

                            // Find previous version of mod.
                            var oldMod = InstalledMods.FirstOrDefault(m => m.Repository == mod.Repository);
                            if (oldMod != null)
                            {
                                Logger.WriteLine("An installed version of this mod was detected.");
                                // This version was already installed.
                                if (oldMod.IsDevelopment || oldMod.Tag == mod.Tag)
                                {
                                    Logger.WriteLine("Installed version is already up to date. Aborting.");
                                    return;
                                }

                                Logger.WriteLine("Deleting installed version.");
                                oldMod.Delete();

                                // Deleting is stupid.
                                // TODO look for better solution
                                await Task.Delay(1000);
                            }

                            // Install mod.
                            Logger.WriteLine("Copying mod to mods folder.");
                            foreach (var entry in zip.Entries)
                            {
                                if (!entry.FullName.StartsWith(mainFolder))
                                    continue;

                                // Compute path.
                                var partDir = entry.FullName.Substring(mainFolder.Length);
                                var path = Path.Combine(mod.InstallationPath, partDir);

                                if (string.IsNullOrEmpty(entry.Name))
                                {
                                    Logger.WriteLine($"Creating directory '{path}'.");
                                    Directory.CreateDirectory(path);
                                }
                                else
                                {
                                    Logger.WriteLine($"Storing mod file '{path}'.");
                                    using (var openStream = entry.Open())
                                    using (var fileStream = File.OpenWrite(path))
                                        await openStream.CopyToAsync(fileStream);
                                }
                            }

                            // Save and compile the mod.
                            mod.Save();
                            mod.Compile();
                        }
                    }
                    break;
                default:
                    throw new Exception("unsupported asset type");
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
