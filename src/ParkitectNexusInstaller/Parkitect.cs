// ParkitectNexusInstaller
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.IO;
using System.Threading.Tasks;
using ParkitectNexusInstaller.Properties;

namespace ParkitectNexusInstaller
{
    internal class Parkitect
    {
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

        public bool IsInstalled => InstallationPath != null;

        private static bool IsValidInstallationPath(string path)
        {
            return !string.IsNullOrWhiteSpace(path) && File.Exists(Path.Combine(path, "Parkitect.exe"));
        }

        public bool SetInstallationPathIfValid(string path)
        {
            if (IsValidInstallationPath(path))
            {
                InstallationPath = path;
                return true;
            }

            return false;
        }

        public bool DetectInstallationPath()
        {
            if (IsInstalled)
                return true;

            // todo: Detect registry key of installation path.

            return false;
        }

        public async Task StoreAsset(ParkitectAsset asset)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));
            if (!IsInstalled)
                throw new Exception("parkitect is not installed");

            var assetInfo = asset.Type.GetCustomAttribute<ParkitectAssetInfoAttribute>();

            if (assetInfo == null)
                throw new Exception("invalid asset type");

            var storagePath = Path.Combine(InstallationPath, assetInfo.StorageFolder);
            var filePath = Path.Combine(storagePath, asset.FileName);
            var attempt = 1;
            var fileName = Path.GetFileNameWithoutExtension(asset.FileName);
            var fileExtension = Path.GetExtension(asset.FileName);

            while (File.Exists(filePath))
                filePath = Path.Combine(storagePath, $"{fileName} ({++attempt}){fileExtension}");

            Directory.CreateDirectory(storagePath);

            using (var fileStream = File.Create(filePath))
            {
                asset.Stream.Seek(0, SeekOrigin.Begin);
                await asset.Stream.CopyToAsync(fileStream);
            }
        }
    }
}