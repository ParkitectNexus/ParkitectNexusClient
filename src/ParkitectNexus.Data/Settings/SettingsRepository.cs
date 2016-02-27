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
        private readonly ILogger _log;
        private readonly string _path;

        public SettingsRepository(T model, ILogger log)
        {
            _log = log;
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
            {
                _log.WriteLine($"Loading settings from {_path} for model {Model}.");
                JsonConvert.PopulateObject(File.ReadAllText(_path), Model);
            }
            else
            {
                _log.WriteLine($"Could not load settings from {_path} for model {Model}, file does not exist.");
            }
        }

        public void Save()
        {
            _log.WriteLine($"Saving setting for model {Model} in {_path}.");
            File.WriteAllText(_path, JsonConvert.SerializeObject(Model));
        }
    }
}
