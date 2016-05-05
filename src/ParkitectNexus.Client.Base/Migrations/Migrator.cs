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
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Assets.Meta;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Client.Base
{
    public class Migrator
    {
        private readonly IParkitect _parkitect;

        public Migrator(IParkitect parkitect)
        {
            _parkitect = parkitect;
        }

        private void MigrateModJsonPreClientVersion2()
        {
            foreach (var mod in _parkitect.Assets[AssetType.Mod])
            {
                var metaPath = Path.Combine(mod.InstallationPath, "modinfo.meta");
                if (!File.Exists(metaPath))
                {
                    var oldData =
                        JsonConvert.DeserializeObject<OldModJsonFile>(
                            File.ReadAllText(Path.Combine(mod.InstallationPath, "mod.json")));

                    var meta = new ModMetadata
                    {
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