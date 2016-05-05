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
using System.IO;
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

        public void SetItem<T>(string name, T item) where T : class
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var path = GetFullPath(name);

            if (item == null)
            {
                if (File.Exists(path))
                    File.Delete(path);

                return;
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(item, _converter));
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