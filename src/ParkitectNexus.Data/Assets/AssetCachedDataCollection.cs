// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ParkitectNexus.Data.Assets
{
    [JsonObject]
    public class AssetCachedDataCollection : IEnumerable<AssetCachedData>
    {
        // todo: getter only?
        [JsonProperty]
        public Dictionary<string, AssetCachedData> Assets { get; set; } = new Dictionary<string, AssetCachedData>();

        public bool Prune(Func<KeyValuePair<string, AssetCachedData>, bool> func)
        {
            var prunes = Assets.Where(asset => !func(asset)).ToArray();

            foreach (var prune in prunes)
            {
                prune.Value.ThumbnailFile?.Delete();
                Assets.Remove(prune.Key);
            }
            return prunes.Any();
        }

        public AssetCachedData GetOrCreate(string key, Func<string, AssetCachedData> func)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            AssetCachedData value;
            if (!Assets.TryGetValue(key, out value))
            {
                value = func(key);
                Assets[key] = value;
            }

            return value;
        }

        #region Implementation of IEnumerable

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<AssetCachedData> GetEnumerator()
        {
            return Assets.Values.OfType<AssetCachedData>().GetEnumerator();
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