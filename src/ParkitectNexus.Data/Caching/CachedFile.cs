// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

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
