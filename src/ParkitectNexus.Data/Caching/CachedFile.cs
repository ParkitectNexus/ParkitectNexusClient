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
using System.Linq;
using Newtonsoft.Json;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Caching
{
    [JsonObject]
    public class CachedFile : ICachedFile
    {
        public CachedFile()
        {
            Path = null;
        }

        public CachedFile(string path)
        {
            Path = path;
        }

        [JsonProperty]
        public string Path { get; set; }

        private string GetFullPath()
        {
            var basePath = System.IO.Path.Combine(AppData.Path, "cache", "file");

            Directory.CreateDirectory(basePath);
            return System.IO.Path.Combine(basePath, Path);
        }

        private void GenerateUniquePath()
        {
            if (Path != null) return;

            do Path = RandomString(16); while (File.Exists(GetFullPath()));
        }

        private static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #region Implementation of ICachedFile

        public bool Exists => File.Exists(GetFullPath());

        public Stream Open(FileMode mode)
        {
            GenerateUniquePath();

            if ((mode == FileMode.Open || mode == FileMode.Append || mode == FileMode.Truncate) && !Exists)
                return null;

            return Path == null ? null : File.Open(GetFullPath(), FileMode.OpenOrCreate);
        }

        public void Delete()
        {
            if (Path == null) return;

            if (File.Exists(GetFullPath()))
                File.Delete(GetFullPath());
            Path = null;
        }

        #endregion
    }
}