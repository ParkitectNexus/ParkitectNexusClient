// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.AssetMagic.Readers;
using ParkitectNexus.Data.Caching;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Assets
{
    public class LocalAssetsRepository : ILocalAssetsRepository
    {
        private readonly ICacheManager _cacheManager;
        private readonly IParkitect _parkitect;
        private readonly ILogger _log;

        public LocalAssetsRepository(ICacheManager cacheManager, IParkitect parkitect, ILogger log)
        {
            if (cacheManager == null) throw new ArgumentNullException(nameof(cacheManager));
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            _cacheManager = cacheManager;
            _parkitect = parkitect;
            _log = log;
        }

        public event EventHandler<AssetEventArgs> AssetAdded;
        public event EventHandler<AssetEventArgs> AssetRemoved;

        public IEnumerable<Asset> GetAssets(AssetType type)
        {
            switch (type)
            {
                case AssetType.Blueprint:
                    return GetAssets("blueprints", _parkitect.Paths.GetAssetPath(AssetType.Blueprint), "*.png", ResolveBlueprintData, AssetType.Blueprint);
                case AssetType.Park:
                    return GetAssets("savegames", _parkitect.Paths.GetAssetPath(AssetType.Park), "*.txt", ResolveSavegameData, AssetType.Park);
                case AssetType.Mod:
                    throw new NotImplementedException();
                default:
                    throw new Exception("Unsupported asset type");
            }
        }

        public int GetAssetCount(AssetType type)
        {
            switch (type)
            {
                case AssetType.Blueprint:
                    return Directory.GetFiles(_parkitect.Paths.GetAssetPath(AssetType.Blueprint), "*.png", SearchOption.AllDirectories).Length;
                case AssetType.Park:
                    return Directory.GetFiles(_parkitect.Paths.GetAssetPath(AssetType.Park), "*.txt", SearchOption.AllDirectories).Length;
                case AssetType.Mod:
                    throw new NotImplementedException();
                default:
                    throw new Exception("Unsupported asset type");

            }
        }


        private IEnumerable<Asset> GetAssets(string cacheKey, string searchPath, string searchPattern,
            Func<string, AssetCachedData> func, AssetType type)
        {
            var assetCachedDataCollection = _cacheManager.GetItemOrNew<AssetCachedDataCollection>(cacheKey);

            foreach(var path in Directory.GetFiles(searchPath, searchPattern, SearchOption.AllDirectories))
            {
                var relativePath = PathUtility.MakeRelativePath(searchPath, path);
                var cachedData = assetCachedDataCollection.GetOrCreate(relativePath, func);
                yield return new Asset(path, cachedData, type);
            }

            assetCachedDataCollection.Prune(r => File.Exists(Path.Combine(searchPath, r.Key)));

            _cacheManager.SetItem(cacheKey, assetCachedDataCollection);
        }

        private AssetCachedData ResolveBlueprintData(string relativePath)
        {
            var path = Path.Combine(_parkitect.Paths.GetAssetPath(AssetType.Blueprint), relativePath);
            var reader = new BlueprintReader();
            var data = new AssetCachedData();

            using (var stream = File.OpenRead(path))
            using (var bitmap = (Bitmap) Image.FromStream(stream))
            {
                data.Name = reader.Read(bitmap).Header.Name;

                using (var resized = ImageUtility.ResizeImage(bitmap, 100, 100))
                using (var memory = new MemoryStream())
                using (var fileStream = data.ThumbnailFile.Open(FileMode.OpenOrCreate))
                {
                    fileStream.SetLength(0);
                    resized.Save(memory, ImageFormat.Bmp);
                    memory.Position = 0;
                    memory.CopyTo(fileStream);
                }
            }

            return data;
        }

        private AssetCachedData ResolveSavegameData(string relativePath)
        {
            var path = Path.Combine(_parkitect.Paths.GetAssetPath(AssetType.Park), relativePath);
            var reader = new SavegameReader();
            var data = new AssetCachedData();

            using (var stream = File.OpenRead(path))
            {
                var savegame = reader.Deserialize(stream);
                var screenshot = savegame.Screenshot;

                data.Name = savegame.Header.Name;

                using (var memory = new MemoryStream())
                using (var fileStream = data.ImageFile.Open(FileMode.OpenOrCreate))
                {
                    fileStream.SetLength(0);
                    screenshot.Save(memory, ImageFormat.Bmp);
                    memory.Position = 0;
                    memory.CopyTo(fileStream);
                }

                using (var resized = ImageUtility.ResizeImage(screenshot, 100, 100))
                using (var memory = new MemoryStream())
                    using(var fileStream = data.ThumbnailFile.Open(FileMode.OpenOrCreate))
                {
                    fileStream.SetLength(0);
                    resized.Save(memory, ImageFormat.Bmp);
                    memory.Position = 0;
                    memory.CopyTo(fileStream);
                }
            }

            return data;
        }

        public async Task StoreAsset(IDownloadedAsset asset)
        {
            switch (asset.ApiAsset.Type)
            {
                case AssetType.Blueprint:
                case AssetType.Park:
                    // Create the directory where the asset should be stored and create a path to where the asset should be stored.
                    var storagePath = _parkitect.Paths.GetAssetPath(asset.ApiAsset.Type);
                    var assetPath = Path.Combine(storagePath, asset.FileName);

                    Directory.CreateDirectory(storagePath);

                    _log.WriteLine($"Storing asset to {assetPath}.");

                    // If the file already exists, add a number behind the file name.
                    if (File.Exists(assetPath))
                    {
                        _log.WriteLine("Asset already exists, comparing hashes.");

                        var md5 = MD5.Create();

                        // Compute hash of downloaded asset to match with installed hash.
                        asset.Stream.Seek(0, SeekOrigin.Begin);
                        var validHash = md5.ComputeHash(asset.Stream);

                        if (validHash.SequenceEqual(md5.ComputeHash(File.OpenRead(assetPath))))
                        {
                            _log.WriteLine("Asset hashes match, aborting.");
                            return;
                        }

                        _log.WriteLine("Asset hashes mismatch, computing new file name.");
                        // Separate the filename and the extension.
                        var attempt = 1;
                        var fileName = Path.GetFileNameWithoutExtension(asset.FileName);
                        var fileExtension = Path.GetExtension(asset.FileName);

                        // Update the path to where the the asset should be stored by adding a number behind the name until an available filename has been found.
                        do
                        {
                            assetPath = Path.Combine(storagePath, $"{fileName} ({++attempt}){fileExtension}");

                            if (File.Exists(assetPath) &&
                                validHash.SequenceEqual(md5.ComputeHash(File.OpenRead(assetPath))))
                                return;
                        } while (File.Exists(assetPath));

                        _log.WriteLine($"Newly computed path is {assetPath}.");
                    }

                    _log.WriteLine("Writing asset to file.");
                    // Write the stream to a file at the asset path.
                    using (var fileStream = File.Create(assetPath))
                    {
                        asset.Stream.Seek(0, SeekOrigin.Begin);
                        await asset.Stream.CopyToAsync(fileStream);
                    }
                    break;
                case AssetType.Mod:
                    _log.WriteLine("Attempting to open mod stream.");
                    using (var zip = new ZipArchive(asset.Stream, ZipArchiveMode.Read))
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
                            var json = await streamReader.ReadToEndAsync();
                            var mod = ObjectFactory.GetInstance<IParkitectMod>();
                            JsonConvert.PopulateObject(json, mod);

                            _log.WriteLine($"mod.json was deserialized to mod object '{mod}'.");

                            // Set default mod properties.
                            mod.Tag = asset.Info.Tag;
                            mod.Repository = asset.Info.Repository;
                            mod.InstallationPath = Path.Combine(_parkitect.Paths.GetAssetPath(AssetType.Mod),
                                asset.Info.Repository.Replace('/', '@'));
                            mod.IsEnabled = true;
                            mod.IsDevelopment = false;

                            // Find previous version of mod.
                            var oldMod = _parkitect.InstalledMods.FirstOrDefault(m => m.Repository == mod.Repository);
                            if (oldMod != null)
                            {
                                _log.WriteLine("An installed version of this mod was detected.");
                                // This version was already installed.
                                if (oldMod.IsDevelopment || oldMod.Tag == mod.Tag)
                                {
                                    _log.WriteLine("Installed version is already up to date. Aborting.");
                                    return;
                                }

                                _log.WriteLine("Deleting installed version.");
                                oldMod.Delete();

                                // Deleting is stupid.
                                // TODO look for better solution
                                await Task.Delay(1000);
                            }

                            // Install mod.
                            _log.WriteLine("Copying mod to mods folder.");
                            foreach (var entry in zip.Entries)
                            {
                                if (!entry.FullName.StartsWith(mainFolder))
                                    continue;

                                // Compute path.
                                var partDir = entry.FullName.Substring(mainFolder.Length);
                                var path = Path.Combine(mod.InstallationPath, partDir);

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

                            // Save and compile the mod.
                            mod.Save();
                            mod.Compile();
                        }
                    }
                    break;
                default:
                    throw new Exception("unknown asset type");
            }

            OnAssetAdded(null);
        }

        protected virtual void OnAssetAdded(AssetEventArgs e)
        {
            AssetAdded?.Invoke(this, e);
        }

        protected virtual void OnAssetRemoved(AssetEventArgs e)
        {
            AssetRemoved?.Invoke(this, e);
        }
    }
}
