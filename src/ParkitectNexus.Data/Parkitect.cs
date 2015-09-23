// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.Data.Properties;

namespace ParkitectNexus.Data
{
    /// <summary>
    ///     Represents the installation directory of the Parkitect game.
    /// </summary>
    public class Parkitect
    {
        /// <summary>
        ///     Gets or sets the installation path.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the installation path is invalid</exception>
        public string InstallationPath
        {
            get
            {
                return IsValidInstallationPath(Settings.Default.InstallationPath)
                    ? Settings.Default.InstallationPath
                    : null;
            }
            set
            {
                if (!IsValidInstallationPath(value))
                    throw new ArgumentException("invalid installation path", nameof(value));

                Settings.Default.InstallationPath = value;
                Settings.Default.Save();
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the game is installed.
        /// </summary>
        public bool IsInstalled => ExecutablePath != null;

        /// <summary>
        ///     Gets the data path.
        /// </summary>
        public string DataPath => !IsInstalled ? null : Path.Combine(InstallationPath, "Parkitect_Data");

        /// <summary>
        /// Gets the executable path.
        /// </summary>
        public string ExecutablePath
                    =>
                        InstallationPath == null || !File.Exists(Path.Combine(InstallationPath, "Parkitect.exe"))
                            ? null
                            : Path.Combine(InstallationPath, "Parkitect.exe");

        /// <summary>
        ///     Gets the mods path.
        /// </summary>
        public string ModsPath
        {
            get
            {
                if (!IsInstalled) return null;
                var path = Path.Combine(InstallationPath, "mods");
                Directory.CreateDirectory(path);
                return path;
            }
        }

        /// <summary>
        ///     Gets the mods file path.
        /// </summary>
        public string ModsFilePath => !IsInstalled ? null : Path.Combine(ModsPath, "mods.json");

        /// <summary>
        ///     Gets the managed data path.
        /// </summary>
        public string ManagedDataPath => !IsInstalled ? null : Path.Combine(DataPath, "Managed");

        public IEnumerable<ParkitectMod> InstalledMods 
        {
            get
            {
                if (!IsInstalled)
                    yield break;

                if (!File.Exists(ModsFilePath))
                    yield break;

                foreach (var name in JsonConvert.DeserializeObject<string[]>(File.ReadAllText(ModsFilePath)))
                {
                    var path = Path.Combine(ModsPath, name);
                    var mod = JsonConvert.DeserializeObject<ParkitectMod>(File.ReadAllText(Path.Combine(path, "mod.json")));
                    mod.Path = path;
                    yield return mod;
                }
            }
        }

        public void InstallMod(string repository)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Determines whether the specified path is valid installation path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>true if valid; false otherwise.</returns>
        private static bool IsValidInstallationPath(string path)
        {
            return !string.IsNullOrWhiteSpace(path) && File.Exists(Path.Combine(path, "Parkitect.exe"));
        }

        /// <summary>
        ///     Sets the installation path if the specified path is a valid installation path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>true if valid; false otherwise.</returns>
        public bool SetInstallationPathIfValid(string path)
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
        public bool DetectInstallationPath()
        {
            if (IsInstalled)
                return true;

            // todo: Detect registry key of installation path.

            return false;
        }

        /// <summary>
        /// Launches the game with the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The launched process.</returns>
        public Process Launch(string arguments = "-single-instance")
        {
            return !IsInstalled
                ? null
                : Process.Start(new ProcessStartInfo(ExecutablePath)
                {
                    WorkingDirectory = InstallationPath,
                    Arguments = arguments
                });
        }

        /// <summary>
        ///     Stores the specified asset in the game's correct directory.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <returns>A task which performs the requested action.</returns>
        public async Task StoreAsset(ParkitectAsset asset)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));
            if (!IsInstalled)
                throw new Exception("parkitect is not installed");

            // Gather information about the asset type.
            var assetInfo = asset.Type.GetCustomAttribute<ParkitectAssetInfoAttribute>();

            if (assetInfo == null)
                throw new Exception("invalid asset type");

            // Create the directory where the asset should be stored and create a path to where the asset should be stored.
            var storagePath = Path.Combine(InstallationPath, assetInfo.StorageFolder);
            var assetPath = Path.Combine(storagePath, asset.FileName);

            Directory.CreateDirectory(storagePath);
            
            // If the file already exists, add a number behind the file name.
            if (File.Exists(assetPath))
            {
                var md5 = MD5.Create();

                // Compute hash of downloaded asset to match with installed hash.
                asset.Stream.Seek(0, SeekOrigin.Begin);
                var validHash = md5.ComputeHash(asset.Stream);

                if (validHash.SequenceEqual(md5.ComputeHash(File.OpenRead(assetPath))))
                    return;

                // Separate the filename and the extension.
                var attempt = 1;
                var fileName = Path.GetFileNameWithoutExtension(asset.FileName);
                var fileExtension = Path.GetExtension(asset.FileName);

                // Update the path to where the the asset should be stored by adding a number behind the name until an available filename has been found.
                do
                {
                    assetPath = Path.Combine(storagePath, $"{fileName} ({++attempt}){fileExtension}");

                    if (File.Exists(assetPath) && validHash.SequenceEqual(md5.ComputeHash(File.OpenRead(assetPath))))
                        return;

                } while ( File.Exists(assetPath));
            }
            
            // Write the stream to a file at the asset path.
            using (var fileStream = File.Create(assetPath))
            {
                asset.Stream.Seek(0, SeekOrigin.Begin);
                await asset.Stream.CopyToAsync(fileStream);
            }
        }
    }
}