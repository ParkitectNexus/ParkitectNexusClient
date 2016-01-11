// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using Newtonsoft.Json;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Settings
{
    public class SettingsRepository<T> : ISettingsRepository<T>
    {
        private readonly string _path;

        public SettingsRepository(T model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            Model = model;
            _path = Path.Combine(AppData.Path, Model.GetType().Name + ".json");
            Load();
        }

        public T Model { get; }

        public void Dispose()
        {
            Save();
        }

        public void Load()
        {
            if (File.Exists(_path))
                JsonConvert.PopulateObject(File.ReadAllText(_path), Model);
        }

        public void Save()
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(Model));
        }
    }
}
