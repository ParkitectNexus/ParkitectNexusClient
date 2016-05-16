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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.Data.Assets.CachedData;
using ParkitectNexus.Data.Assets.Meta;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;

namespace ParkitectNexus.Data.Assets
{
    public class LocalAssetRepository : ILocalAssetRepository
    {
        private readonly IAssetCachedDataStorage _assetCachedDataStorage;
        private readonly IAssetMetadataStorage _assetMetadataStorage;
        private readonly ILogger _log;
        private readonly IParkitect _parkitect;
        private readonly IWebsite _website;

        public LocalAssetRepository(IParkitect parkitect, IWebsite website, ILogger log, IAssetMetadataStorage assetMetadataStorage,
            IAssetCachedDataStorage assetCachedDataStorage)
        {
            _parkitect = parkitect;
            _website = website;
            _log = log;
            _assetMetadataStorage = assetMetadataStorage;
            _assetCachedDataStorage = assetCachedDataStorage;
        }

        private string[] GetFilesInAssetPath(AssetType type)
        {
            switch (type)
            {
                case AssetType.Blueprint:
                    if (_parkitect.Paths.GetAssetPath(type) == null)
                        return new string[0];

                    return Directory.GetFiles(_parkitect.Paths.GetAssetPath(type), "*.png",
                        SearchOption.AllDirectories);
                case AssetType.Savegame:
                    if (_parkitect.Paths.GetAssetPath(type) == null)
                        return new string[0];

                    return Directory.GetFiles(_parkitect.Paths.GetAssetPath(type), "*.txt",
                        SearchOption.AllDirectories)
                        .Concat(Directory.GetFiles(_parkitect.Paths.GetAssetPath(type), "*.park",
                            SearchOption.AllDirectories)).ToArray();
                case AssetType.Mod:
                    if (_parkitect.Paths.GetAssetPath(type) == null)
                        return new string[0];
                    return Directory.GetDirectories(_parkitect.Paths.GetAssetPath(AssetType.Mod))
                        .Where(path => File.Exists(Path.Combine(path, "mod.json")))
                        .ToArray();
                default:
                    throw new Exception("unsupported asset type.");
            }
        }

        #region Implementation of ILocalAssetRepository

        public event EventHandler<AssetEventArgs> AssetAdded;

        public event EventHandler<AssetEventArgs> AssetRemoved;

        public IEnumerable<Asset> this[AssetType type]
        {
            get
            {
                switch (type)
                {
                    case AssetType.Blueprint:
                        foreach (var path in GetFilesInAssetPath(type))
                        {
                            var metadata = _assetMetadataStorage.GetMetadata(type, path);
                            var cachedData = _assetCachedDataStorage.GetData(type, metadata, path).Result;
                            yield return new BlueprintAsset(path, metadata, cachedData);
                        }
                        break;
                    case AssetType.Savegame:
                        foreach (var path in GetFilesInAssetPath(type))
                        {
                            var metadata = _assetMetadataStorage.GetMetadata(type, path);
                            var cachedData =
                                _assetCachedDataStorage.GetData(type, metadata, path).Result as AssetWithImageCachedData;
                            yield return new SavegameAsset(path, metadata, cachedData);
                        }
                        break;
                    case AssetType.Mod:
                        foreach (var path in GetFilesInAssetPath(type))
                        {
                            ModAsset result = null;
                            try
                            {
                                var metadata = _assetMetadataStorage.GetMetadata(type, path) as IModMetadata;

                                var modInformationString = File.ReadAllText(Path.Combine(path, "mod.json"));
                                var modInformation = JsonConvert.DeserializeObject<ModInformation>(modInformationString);

                                var cachedData = metadata == null
                                    ? new AssetWithImageCachedData()
                                    : _assetCachedDataStorage.GetData(type, metadata, path).Result as
                                        AssetWithImageCachedData;

                                result = new ModAsset(path, metadata, cachedData, modInformation);
                            }
                            catch (Exception e)
                            {
                                _log.WriteLine($"Failed loading mod at path {path}", LogLevel.Fatal);
                                _log.WriteException(e);
                            }

                            if (result != null)
                                yield return result;
                        }
                        break;
                    default:
                        throw new Exception("Unsupported asset type");
                }
            }
        }

        public int GetAssetCount(AssetType type)
        {
            return GetFilesInAssetPath(type).Length;
        }

        public async Task<IAsset> StoreAsset(IDownloadedAsset downloadedAsset)
        {
            switch (downloadedAsset.ApiAsset.Type)
            {
                case AssetType.Blueprint:
                case AssetType.Savegame:
                {
                    // Create the directory where the asset should be stored and create a path to where the asset should be stored.
                    var storagePath = _parkitect.Paths.GetAssetPath(downloadedAsset.ApiAsset.Type);
                    var assetPath = Path.Combine(storagePath, downloadedAsset.FileName);
                    Directory.CreateDirectory(storagePath);

                    _log.WriteLine($"Storing asset to {assetPath}.");

                    // If the file already exists, add a number behind the file name.
                    if (File.Exists(assetPath))
                    {
                        _log.WriteLine("Asset already exists, comparing hashes.");

                        // Compute hash of downloaded asset to match with installed hash.
                        var validHash = downloadedAsset.Stream.CreateMD5Checksum();

                        if (validHash.SequenceEqual(File.OpenRead(assetPath).CreateMD5Checksum()))
                        {
                            _log.WriteLine("Asset hashes match, aborting.");
                            return null;
                        }

                        _log.WriteLine("Asset hashes mismatch, computing new file name.");
                        // Separate the filename and the extension.
                        var attempt = 1;
                        var fileName = Path.GetFileNameWithoutExtension(downloadedAsset.FileName);
                        var fileExtension = Path.GetExtension(downloadedAsset.FileName);

                        // Update the path to where the the asset should be stored by adding a number behind the name until an available filename has been found.
                        do
                        {
                            assetPath = Path.Combine(storagePath, $"{fileName} ({++attempt}){fileExtension}");

                            if (File.Exists(assetPath) &&
                                validHash.SequenceEqual(File.OpenRead(assetPath).CreateMD5Checksum()))
                                return null;
                        } while (File.Exists(assetPath));

                        _log.WriteLine($"Newly computed path is {assetPath}.");
                    }

                    _log.WriteLine("Writing asset to file.");
                    // Write the stream to a file at the asset path.
                    using (var fileStream = File.Create(assetPath))
                    {
                        downloadedAsset.Stream.Seek(0, SeekOrigin.Begin);
                        await downloadedAsset.Stream.CopyToAsync(fileStream);
                    }

                    var meta = new AssetMetadata
                    {
                        Id = downloadedAsset.ApiAsset.Id
//                        InstalledVersion = downloadedAsset.ApiAsset.UpdatedAt
                    };// TODO: Re-add installed version

                    _assetMetadataStorage.StoreMetadata(downloadedAsset.ApiAsset.Type, assetPath, meta);
                    var cachedData =
                        await _assetCachedDataStorage.GetData(downloadedAsset.ApiAsset.Type, meta, assetPath);

                    Asset createdAsset = null;
                    switch (downloadedAsset.ApiAsset.Type)
                    {
                        case AssetType.Blueprint:
                            createdAsset = new BlueprintAsset(assetPath, meta, cachedData);
                            break;
                        case AssetType.Savegame:
                            createdAsset = new SavegameAsset(assetPath, meta, cachedData as AssetWithImageCachedData);
                            break;
                    }

                    OnAssetAdded(new AssetEventArgs(createdAsset));
                    return createdAsset;
                }
                case AssetType.Mod:
                {
                    _log.WriteLine("Attempting to open mod stream.");
                    using (var zip = new ZipArchive(downloadedAsset.Stream, ZipArchiveMode.Read))
                    {
                        // Compute name of main directory inside archive.
                        var mainFolder = zip.Entries.FirstOrDefault()?.FullName;
                        if (mainFolder == null)
                            throw new Exception("invalid archive");

                        _log.WriteLine($"Mod archive main folder is {mainFolder}.");

                        // Find the mod.json file. Yay for / \ path divider compatibility.
                        var modJsonPath = Path.Combine(mainFolder, "mod.json").Replace('/', '\\');
                        var modJson = zip.Entries.FirstOrDefault(e => e.FullName.Replace('/', '\\') == modJsonPath);

                        // Read mod.json.
                        if (modJson == null) throw new Exception("mod is missing mod.json file");
                        using (var streamReader = new StreamReader(modJson.Open()))
                        {
                            var modInformationString = await streamReader.ReadToEndAsync();
                            var modInformation = JsonConvert.DeserializeObject<ModInformation>(modInformationString);

                            var meta = new ModMetadata
                            {
                                Id = downloadedAsset.ApiAsset.Id,
//                                InstalledVersion = downloadedAsset.ApiAsset.UpdatedAt,
                                Tag = downloadedAsset.Info.Tag,
                                Repository = downloadedAsset.Info.Repository
                            }; // TODO: Re-add installed version

                            var installationPath = Path.Combine(_parkitect.Paths.GetAssetPath(AssetType.Mod),
                                downloadedAsset.Info.Repository.Replace('/', '@'));

                            // TODO: Should actually try and look if the mod has been updated since and delete the whole folder.
                            if (Directory.Exists(installationPath))
                            {
                                if (File.Exists(Path.Combine(installationPath, "modinfo.meta")))
                                    File.Delete(Path.Combine(installationPath, "modinfo.meta"));
                                if (File.Exists(Path.Combine(installationPath, "moddata.cache")))
                                    File.Delete(Path.Combine(installationPath, "moddata.cache"));
                            }

                            _log.WriteLine($"mod.json was deserialized to mod object '{modInformation}'.");

                            // Set default mod properties.
                            modInformation.IsEnabled = true;
                            modInformation.IsDevelopment = false;

                            // Find previous version of mod.
                            // TODO uninstall previous versions

                            // Install mod.
                            _log.WriteLine("Copying mod to mods folder.");
                            foreach (var entry in zip.Entries)
                            {
                                if (!entry.FullName.StartsWith(mainFolder))
                                    continue;

                                // Compute path.
                                var partDir = entry.FullName.Substring(mainFolder.Length);
                                var path = Path.Combine(installationPath, partDir);

                                if (partDir == "moddata.cache" || partDir == "modinfo.meta")
                                    continue;

                                if (string.IsNullOrEmpty(entry.Name))
                                {
                                    _log.WriteLine($"Creating directory '{path}'.");
                                    Directory.CreateDirectory(path);
                                }
                                else
                                {
                                    _log.WriteLine($"Storing mod file '{path}'.");
                                    using (var openStream = entry.Open())
                                    using (var fileStream = File.OpenWrite(path))
                                        await openStream.CopyToAsync(fileStream);
                                }
                            }

                            _log.WriteLine("Register installation to API.");
                            _website.API.RegisterDownload(downloadedAsset.ApiAsset.Id);

                            _assetMetadataStorage.StoreMetadata(downloadedAsset.ApiAsset.Type, installationPath, meta);
                            var cachedData = await _assetCachedDataStorage.GetData(downloadedAsset.ApiAsset.Type, meta,
                                installationPath);

                            modInformationString = JsonConvert.SerializeObject(modInformation);
                            File.WriteAllText(Path.Combine(installationPath, "mod.json"), modInformationString);

                            var createdAsset = new ModAsset(installationPath, meta,
                                cachedData as AssetWithImageCachedData, modInformation);
                            OnAssetAdded(new AssetEventArgs(createdAsset));

                            return createdAsset;
                        }
                    }
                }
                default:
                    throw new Exception("unknown asset type");
            }
        }

        public void DeleteAsset(IAsset asset)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));

            _log.WriteLine($"Deleting asset {asset}");

            switch (asset.Type)
            {
                case AssetType.Blueprint:
                case AssetType.Savegame:
                    var directory = Path.GetDirectoryName(asset.InstallationPath);
                    var filenameWithoutExtension = Path.GetFileNameWithoutExtension(asset.InstallationPath);

                    try
                    {
                        File.Delete(asset.InstallationPath);
                        File.Delete(Path.Combine(directory, $"{filenameWithoutExtension}.cache"));
                        File.Delete(Path.Combine(directory, $"{filenameWithoutExtension}.meta"));

                        OnAssetRemoved(new AssetEventArgs(asset));
                    }
                    catch (Exception e)
                    {
                        _log.WriteLine($"Failed to delete asset {asset.Name}", LogLevel.Fatal);
                        _log.WriteException(e);
                    }
                    break;
                case AssetType.Mod:
                    try
                    {
                        Directory.Delete(asset.InstallationPath, true);

                        OnAssetRemoved(new AssetEventArgs(asset));
                    }
                    catch (Exception e)
                    {
                        _log.WriteLine($"Failed to delete mod {asset.Name}", LogLevel.Fatal);
                        _log.WriteException(e);
                    }
                    break;
            }
        }

        #endregion

        #region Event raisers

        /// <summary>
        ///     Raises the <see cref="E:AssetAdded" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ParkitectNexus.Data.Assets.AssetEventArgs" /> instance containing the event data.</param>
        protected virtual void OnAssetAdded(AssetEventArgs e)
        {
            AssetAdded?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="E:AssetRemoved" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ParkitectNexus.Data.Assets.AssetEventArgs" /> instance containing the event data.</param>
        protected virtual void OnAssetRemoved(AssetEventArgs e)
        {
            AssetRemoved?.Invoke(this, e);
        }

        #endregion

        #region Implementation of IEnumerable

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IAsset> GetEnumerator()
        {
            return
                Enum.GetValues(typeof (AssetType))
                    .OfType<AssetType>()
                    .SelectMany(type => this[type])
                    .GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
