// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using Newtonsoft.Json;

namespace ParkitectNexus.Data.Settings
{
    public class Repository<T> : IRepository<T>
    {
        private readonly string _path;

        public Repository(IPathResolver pathResolver, T model)
        {
            if (pathResolver == null) throw new ArgumentNullException(nameof(pathResolver));
            if (model == null) throw new ArgumentNullException(nameof(model));
            Model = model;
            _path = Path.Combine(pathResolver.AppData(), Model.GetType().Name + ".json");
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
