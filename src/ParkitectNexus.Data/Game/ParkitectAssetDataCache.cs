using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace ParkitectNexus.Data.Game
{
    [JsonObject]
    public class ParkitectAssetDataCache<T> : IParkitectAssetDataCache<T> where T : new()
    {
        private readonly string _path;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public ParkitectAssetDataCache(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            _path = path;

            Reload();
        }

        [JsonProperty]
        public IDictionary<string, T> Data { get; set; }

        public void Save()
        {
            var data = JsonConvert.SerializeObject(Data);
            try
            {
                File.WriteAllText(_path, data);
            }
            catch
            {
                // On error keep cache only in session.
            }
        }

        #region Implementation of IParkitectAssetDataCache

        public T GetCachedData(string path, Func<string, T> resolver)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            T name;
            if (!Data.TryGetValue(path, out name))
            {
                name = resolver(path);

                if (name != null)
                    Data[path] = name;
            }
            return name;
        }

        public void ClearCachedData(IEnumerable<string> except)
        {
            foreach (var path in Data.Keys.Where(k => !except.Contains(k)).ToArray())
                Data.Remove(path);
        }


        public void Reload()
        {
            new Dictionary<string, string>();
            if (File.Exists(_path))
            {
                try
                {
                    Data = JsonConvert.DeserializeObject<IDictionary<string, T>>(File.ReadAllText(_path));
                }
                catch
                {
                    // Dispose of cache on error.
                }
            }
            if(Data == null)
            {
                Data = new Dictionary<string, T>();
            }
        }

        #endregion
    }
}
