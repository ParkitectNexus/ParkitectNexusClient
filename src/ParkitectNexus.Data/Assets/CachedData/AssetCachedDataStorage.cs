// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.AssetMagic.Readers;
using ParkitectNexus.Data.Assets.Meta;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Assets.CachedData
{
    public class AssetCachedDataStorage : IAssetCachedDataStorage
    {
        private readonly IWebsite _website;

        ILogger _log;

        public AssetCachedDataStorage(IWebsite website, ILogger log)
        {
            _log = log;
            _website = website;
        }

        public async Task<IAssetCachedData> GetData(AssetType type, IAssetMetadata metadata, string path)
        {
            var metaType = GetCachedDataType(type);
            var metaDataPath = GetCachedDataFilePath(type, path);

            if (!File.Exists(metaDataPath))
            {
                var newInstance = await GenerateCachableData(type, metadata, path);

                if (newInstance == null)
                    return null;

                StoreData(type, path, newInstance);
                return newInstance;
            }

            var data = File.ReadAllText(metaDataPath);
            return JsonConvert.DeserializeObject(data, metaType) as IAssetCachedData;
        }

        public void StoreData(AssetType type, string path, IAssetCachedData metadata)
        {
            var metaDataPath = GetCachedDataFilePath(type, path);

            var data = JsonConvert.SerializeObject(metadata);
            File.WriteAllText(metaDataPath, data);
        }

        private async Task<IAssetCachedData> GenerateCachableData(AssetType type, IAssetMetadata metadata, string path)
        {
            try
            {
                switch (type)
                {
                    case AssetType.Blueprint:
                        using(var blueprintImage = Image.FromFile(path))
                        {
                            var blueprintReader = new BlueprintReader();
                            var blueprint = blueprintReader.Read((Bitmap)blueprintImage);
                            return new AssetCachedData {
                                Name = blueprint.Header.Name
                            };
                        }
                    case AssetType.Mod:
                        if (metadata == null)
                            return null;

                        using (var stream = new MemoryStream())
                        {
                            var asset = await _website.API.GetAsset(metadata.Id);
                            using (var thumbnail = await asset.Thumbnail.Get())
                            {
                                thumbnail.Save(stream, ImageFormat.Png);
                            }

                            return new AssetWithImageCachedData
                            {
                                ImageBase64 = Convert.ToBase64String(stream.ToArray())
                            };
                        }
                    case AssetType.Savegame:
                        var savegameReader = new SavegameReader();
                        var savegame = savegameReader.Deserialize(File.ReadAllText(path));

                        using (var stream = new MemoryStream())
                        {
                            using (var thumbnail = savegame.Screenshot)
                            {
                                thumbnail.Save(stream, ImageFormat.Png);
                            }

                            return new AssetWithImageCachedData
                            {
                                Name = savegame.Header.Name,
                                ImageBase64 = Convert.ToBase64String(stream.ToArray())
                            };
                        }
                    default:
                        throw new Exception("Unsupported type");
                }
            }
            catch(Exception e)
            {
                _log.WriteException(e);
                return new AssetCachedData
                {
                    Name = "Failed to open " + path
                };
            }
        }

        private Type GetCachedDataType(AssetType type)
        {
            switch (type)
            {
                case AssetType.Blueprint:
                    return typeof (AssetCachedData);
                case AssetType.Savegame:
                case AssetType.Mod:
                    return typeof (AssetWithImageCachedData);
                default:
                    throw new Exception("Unsupported asset type");
            }
        }

        private string GetCachedDataFilePath(AssetType type, string path)
        {
            switch (type)
            {
                case AssetType.Blueprint:
                case AssetType.Savegame:
                    return Path.Combine(Path.GetDirectoryName(path), $"{Path.GetFileNameWithoutExtension(path)}.cache");
                case AssetType.Mod:
                    return Path.Combine(path, "moddata.cache");
                default:
                    throw new Exception("Unsupported asset type");
            }
        }
    }
}
