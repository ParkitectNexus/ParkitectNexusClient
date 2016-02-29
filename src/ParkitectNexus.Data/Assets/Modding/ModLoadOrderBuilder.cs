// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

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
        private readonly IParkitect _parkitect;
        private readonly ILogger _log;

        public ModLoadOrderBuilder(IParkitect parkitect, ILogger log)
        {
            _parkitect = parkitect;
            _log = log;
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
    }
}
