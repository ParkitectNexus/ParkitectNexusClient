using System;
using ParkitectNexus.Data.Game;
using System.Collections.Generic;
using Newtonsoft.Json;
using ParkitectNexus.Data.Assets.Meta;
using System.IO;

namespace ParkitectNexus.Client.Base
{
    public class Migrator
    {
        private IParkitect _parkitect;

        public Migrator(IParkitect parkitect)
        {
            _parkitect = parkitect;
        }

        private void MigrateModJsonPreClientVersion2()
        {
            foreach(var mod in _parkitect.Assets[ParkitectNexus.Data.Assets.AssetType.Mod])
            {
                var metaPath = Path.Combine(mod.InstallationPath, "modinfo.meta");
                if(!File.Exists(metaPath))
                {
                    var oldData = JsonConvert.DeserializeObject<OldModJsonFile>(File.ReadAllText(Path.Combine(mod.InstallationPath, "mod.json")));

                    var meta = new ModMetadata {
                        Tag = oldData.Tag,
                        Repository = oldData.Repository,
                        Id = null,
                        InstalledVersion = DateTime.Now
                    };

                    File.WriteAllText(metaPath, JsonConvert.SerializeObject(meta));
                }
            }
        }

        public void Migrate()
        {
            MigrateModJsonPreClientVersion2();
        }

        private class OldModJsonFile
        {
            [JsonProperty]
            public string Tag { get; set; }
            [JsonProperty]
            public string Repository { get; set; }
        }
    }
}

