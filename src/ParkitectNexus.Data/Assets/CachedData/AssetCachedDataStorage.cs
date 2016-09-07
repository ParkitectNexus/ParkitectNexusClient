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
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.AssetMagic.Converters;
using ParkitectNexus.Data.Assets.Meta;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.Client;

namespace ParkitectNexus.Data.Assets.CachedData
{
    public class AssetCachedDataStorage : IAssetCachedDataStorage
    {
        private readonly IWebsite _website;

        private readonly ILogger _log;

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
                        var blueprint = BlueprintConverter.DeserializeFromFile(path);
                        return new AssetCachedData
                        {
                            Name = blueprint.Header.Name
                        };
                    case AssetType.Mod:
                        if (metadata?.Id == null)
                            return new AssetWithImageCachedData();

                        var asset = await _website.API.GetAsset(metadata.Id);
                        using (var memoryStream = new MemoryStream())
                        {
                            try
                            {
                                using (var image = await asset.Image.Data.Get())
                                {
                                    if (image == null)
                                    {
                                        return new AssetWithImageCachedData
                                        {
                                            ImageBase64 = null
                                        };
                                    }

                                    image.Save(memoryStream, ImageFormat.Png);
                                    return new AssetWithImageCachedData
                                    {
                                        ImageBase64 = Convert.ToBase64String(memoryStream.ToArray())
                                    };
                                }
                            }
                            catch (Exception e)
                            {
                                _log.WriteLine("Failed to load image!");
                                _log.WriteException(e);
                                return new AssetWithImageCachedData();
                            }
                        }
                    case AssetType.Savegame:
                        var savegame = SavegameConverter.DeserializeFromFile(path);

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
                    case AssetType.Scenario:
                        var scenario = SavegameConverter.DeserializeFromFile(path);

                        using (var stream = new MemoryStream())
                        {
                            using (var thumbnail = scenario.Screenshot)
                            {
                                thumbnail.Save(stream, ImageFormat.Png);
                            }

                            return new AssetWithImageCachedData
                            {
                                Name = scenario.Header.Name,
                                ImageBase64 = Convert.ToBase64String(stream.ToArray())
                            };
                        }
                    default:
                        throw new Exception("Unsupported type");
                }
            }
            catch (Exception e)
            {
                _log.WriteException(e);
                return new AssetCachedData
                {
                    Name = "Failed to open image for " + path
                };
            }
        }

        private Type GetCachedDataType(AssetType type)
        {
            switch (type)
            {
                case AssetType.Blueprint:
                    return typeof (AssetCachedData);
                case AssetType.Scenario:
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
                case AssetType.Scenario:
                    return Path.Combine(Path.GetDirectoryName(path), $"{Path.GetFileNameWithoutExtension(path)}.cache");
                case AssetType.Mod:
                    return Path.Combine(path, "moddata.cache");
                default:
                    throw new Exception("Unsupported asset type");
            }
        }
    }
}
