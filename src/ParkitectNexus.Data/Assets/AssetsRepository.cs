// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ParkitectNexus.AssetMagic.Readers;
using ParkitectNexus.Data.Caching;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Assets
{
    public class AssetsRepository : IAssetsRepository
    {
        private readonly ICacheManager _cacheManager;
        private readonly IParkitect _parkitect;

        public AssetsRepository(ICacheManager cacheManager, IParkitect parkitect)
        {
            if (cacheManager == null) throw new ArgumentNullException(nameof(cacheManager));
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            _cacheManager = cacheManager;
            _parkitect = parkitect;
        }

        public IEnumerable<Asset> GetBlueprints()
        {
            return GetAssets("blueprints", _parkitect.Paths.Blueprints, "*.png", ResolveBlueprintData, ParkitectAssetType.Blueprint);
        }

        public IEnumerable<Asset> GetSavegames()
        {
            return GetAssets("savegames", _parkitect.Paths.Savegames, "*.txt", ResolveSavegameData, ParkitectAssetType.Savegame);
        }

        public int GetBlueprintsCount()
        {
            return Directory.GetFiles(_parkitect.Paths.Blueprints, "*.png", SearchOption.AllDirectories).Length;
        }

        public int GetSavegamesCount()
        {
            return Directory.GetFiles(_parkitect.Paths.Savegames, "*.txt", SearchOption.AllDirectories).Length;
        }

        private IEnumerable<Asset> GetAssets(string cacheKey, string searchPath, string searchPattern,
            Func<string, AssetCachedData> func, ParkitectAssetType type)
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
            var path = Path.Combine(_parkitect.Paths.Blueprints, relativePath);
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
            var path = Path.Combine(_parkitect.Paths.Savegames, relativePath);
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
    }
}
