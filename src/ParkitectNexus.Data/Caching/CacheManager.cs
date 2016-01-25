// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Caching
{
    public class CacheManager : ICacheManager
    {
        private readonly CachedFileJsonConverter _converter = new CachedFileJsonConverter();

        public T GetItem<T>(string name) where T : class
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var path = GetFullPath(name);

            try
            {
                return !File.Exists(path)
                    ? null
                    : JsonConvert.DeserializeObject<T>(File.ReadAllText(path), _converter);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public T GetItemOrNew<T>(string name) where T : class
        {
            return GetItem<T>(name) ?? Activator.CreateInstance<T>();
        }

        public void SetItem<T>(string name, T item) where T : class
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (item == null) throw new ArgumentNullException(nameof(item));

            var path = GetFullPath(name);

            File.WriteAllText(path, JsonConvert.SerializeObject(item, _converter));
        }

        public void Clear()
        {
            var path = Path.Combine(AppData.Path, "cache");
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        private string GetFullPath(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            var basePath = Path.Combine(AppData.Path, "cache", "data");

            Directory.CreateDirectory(basePath);
            return Path.Combine(basePath, name);
        }
    }
}
