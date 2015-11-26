using System;
using ParkitectNexus.Data.Utilities;
using System.IO;
using Newtonsoft.Json;

namespace ParkitectNexus.Data.Settings
{
    public abstract class SettingsBase
    {
        private string _path;
        public SettingsBase ()
        {
            _path = Path.Combine(AppData.Path, GetType().Name + ".json");

            Load ();
        }

        public void Load()
        {

            if(File.Exists(_path))
                JsonConvert.PopulateObject (File.ReadAllText(_path), this);
        }

        public void Save()
        {
            File.WriteAllText (_path, JsonConvert.SerializeObject (this));
        }
    }

    public class GameSettings : SettingsBase
    {
        public string InstallationPath { get; set; }
    }
}

