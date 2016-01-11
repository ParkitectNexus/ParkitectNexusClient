

using ParkitectNexus.Data.Settings;
using System.IO;
using System;
using Newtonsoft.Json;
using StructureMap.TypeRules;
using System.Collections.Generic;

namespace ParkitectNexus.Data.Settings
{
    public class Repository<T> : IRepository<T>
    {
        private readonly string _path;
        private IPathResolver _pathResolver;
        private T _model;

        public T Model
        {
            get
            {
                return _model;
            }
        }

        public Repository(IPathResolver pathResolver, T model)
        {
            _model = model;
            _pathResolver = pathResolver;
            _path = Path.Combine(pathResolver.AppData(), _model.GetType().Name + ".json");
            Load();
        }

        public void Dispose()
        {
            this.Save();
        }

        public void Load()
        {
            if (File.Exists(_path))
                JsonConvert.PopulateObject(File.ReadAllText(_path), _model);
        }

        public void Save()
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(_model));
        }
    }
}
