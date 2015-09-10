// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.IO;
using System.Threading.Tasks;
using ParkitectNexusClient.Properties;

namespace ParkitectNexusClient
{
    /// <summary>
    ///     Represents the installation directory of the Parkitect game.
    /// </summary>
    internal class Parkitect
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
        public bool IsInstalled => InstallationPath != null;

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
                // Separate the filename and the extension.
                var attempt = 1;
                var fileName = Path.GetFileNameWithoutExtension(asset.FileName);
                var fileExtension = Path.GetExtension(asset.FileName);

                // Update the path to where the the asset should be stored by adding a number behind the name until an available filename has been found.
                do assetPath = Path.Combine(storagePath, $"{fileName} ({++attempt}){fileExtension}"); while (
                    File.Exists(assetPath));
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