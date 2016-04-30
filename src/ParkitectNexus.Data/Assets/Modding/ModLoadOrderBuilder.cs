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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Assets.Modding
{
    public class ModLoadOrderBuilder : IModLoadOrderBuilder
    {
        private readonly ILogger _log;
        private readonly IParkitect _parkitect;

        public ModLoadOrderBuilder(IParkitect parkitect, ILogger log)
        {
            _parkitect = parkitect;
            _log = log;
        }

        public IEnumerable<IModAsset> Build()
        {
            _log.WriteLine("Building mod load order list.");

            var mods = _parkitect.Assets[AssetType.Mod].OfType<IModAsset>().ToArray();

            var orderedList = new List<IModAsset>();

            foreach (var mod in mods)
            {
                if (mod.Information.IsEnabled || mod.Information.IsDevelopment)
                    AddModToList(mods, new Stack<IModAsset>(), mod, orderedList);
            }

            return orderedList;
        }

        public void BuildAndStore()
        {
            File.WriteAllLines(Path.Combine(_parkitect.Paths.GetAssetPath(AssetType.Mod), "load.dat"),
                Build().Select(m => Path.GetFileName(m.InstallationPath)).ToArray());
        }

        private static void AddModToList(IModAsset[] allMods, Stack<IModAsset> buildStack, IModAsset mod,
            IList<IModAsset> orderedList)
        {
            if (orderedList.Contains(mod))
                return;

            if (mod.Information.Dependencies != null)
            {
                buildStack.Push(mod);

                foreach (var dependency in mod.Information.Dependencies)
                {
                    if (buildStack.Any(m => m.Repository == dependency))
                        throw new Exception("Curcular dependency detected");

                    var dependencyMod = allMods.FirstOrDefault(m => m.Repository == dependency);

                    if (dependencyMod != null)
                        AddModToList(allMods, buildStack, dependencyMod, orderedList);
                }

                buildStack.Pop();
            }

            orderedList.Add(mod);
        }
    }
}