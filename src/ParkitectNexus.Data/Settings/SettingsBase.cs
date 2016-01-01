﻿// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System.IO;
using Newtonsoft.Json;
using ParkitectNexus.Data.Utilities;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace ParkitectNexus.Data.Settings
{
	[DataContract]
    public abstract class SettingsBase
    {
		protected readonly string _path;

        protected SettingsBase()
        {
            _path = Path.Combine(AppData.Path, GetType().Name + ".json");

			Load();
        }

		public void Load()
        {
            if (File.Exists(_path))
                JsonConvert.PopulateObject(File.ReadAllText(_path), this);
        }

		public virtual void Save()
        {
			File.WriteAllText (_path,JsonConvert.SerializeObject(this));
        }
			
    }
}