// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using Newtonsoft.Json;

namespace ParkitectNexus.Data.Assets.Meta
{
    public class AssetMetadataStorage : IAssetMetadataStorage
    {
        public IAssetMetadata GetMetadata(AssetType type, string path)
        {
            var metaType = GetMetadataType(type);
            var metaDataPath = GetMetadataFilePath(type, path);

            if (!File.Exists(metaDataPath))
                return null;

            var data = File.ReadAllText(metaDataPath);
            return JsonConvert.DeserializeObject(data, metaType) as IAssetMetadata;
        }

        public void StoreMetadata(AssetType type, string path, IAssetMetadata metadata)
        {
            var metaDataPath = GetMetadataFilePath(type, path);

            var data = JsonConvert.SerializeObject(metadata);
            File.WriteAllText(metaDataPath, data);
        }

        private static Type GetMetadataType(AssetType type)
        {
            switch (type)
            {
                case AssetType.Blueprint:
                case AssetType.Savegame:
                    return typeof (AssetMetadata);
                case AssetType.Mod:
                    return typeof (ModMetadata);
                default:
                    throw new Exception("Unsupported asset type");
            }
        }

        private static string GetMetadataFilePath(AssetType type, string path)
        {
            switch (type)
            {
                case AssetType.Blueprint:
                case AssetType.Savegame:
                    return Path.Combine(Path.GetDirectoryName(path), $"{Path.GetFileNameWithoutExtension(path)}.meta");
                case AssetType.Mod:
                    return Path.Combine(path, "modinfo.meta");
                default:
                    throw new Exception("Unsupported asset type");
            }
        }
    }
}
