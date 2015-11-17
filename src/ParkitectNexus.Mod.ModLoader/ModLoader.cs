// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using MiniJSON;
using UnityEngine;

namespace ParkitectNexus.Mod.ModLoader
{
    public class ModLoader : IMod
    {
        /// <summary>
        ///     The mods loaded by the ParkitectNexus mod loader.
        /// </summary>
        private readonly List<IMod> _loadedMods = new List<IMod>();

        /// <summary>
        ///     A reference to the list of mod entries as found in the mod manager.
        /// </summary>
        private readonly List<ModManager.ModEntry> _modEntries;

        private GameObject _gameObject;
        private bool _isEnabled;

        public ModLoader()
        {
            _modEntries = typeof (ModManager).GetField("modEntries", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(ModManager.Instance) as List<ModManager.ModEntry>;

            LoadMods();
        }

        private static T ReadFromDictonary<T>(IDictionary<string, object> dictionary, string key)
        {
            object o;
            return dictionary.TryGetValue(key, out o) ? (o is T ? (T) o : default(T)) : default(T);
        }

        private static void SetProperty<T>(object @object, string propertyName, T value)
        {
            var property = @object.GetType()
                .GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null,
                    typeof (T), new Type[0], null);

            if (property != null)
                property.SetValue(@object, value, BindingFlags.NonPublic | BindingFlags.Public, null, null,
                    CultureInfo.CurrentCulture);
        }

        public void LoadMods()
        {
            // Unload mods loaded by this mod loader.
            foreach (var modEntry in _modEntries.ToArray())
            {
                if (!_loadedMods.Contains(modEntry.mod))
                    continue;

                if (_isEnabled)
                    modEntry.disableMod();
                _modEntries.Remove(modEntry);
            }

            _loadedMods.Clear();

            // Compute paths to the mods directory.
            var modsPath = FilePaths.getFolderPath("pnmods");

            // Find mod directories in the mods directory.
            foreach (var folder in Directory.GetDirectories(modsPath))
            {
                try
                {
                    var directoryName = System.IO.Path.GetFileName(folder);
                    var filePath = System.IO.Path.Combine(folder, "mod.json");

                    if (!File.Exists(filePath))
                        continue;

                    // Read the mod.json file.
                    var dictionary = Json.Deserialize(File.ReadAllText(filePath)) as Dictionary<string, object>;

                    if (dictionary == null)
                        continue;

                    var isEnabled = ReadFromDictonary<bool>(dictionary, "IsEnabled");
                    var isDevelopment = ReadFromDictonary<bool>(dictionary, "IsDevelopment");

                    var binBuildPath = System.IO.Path.Combine(folder, "bin/build.dat");

                    // If the mod is not enabled or in development, continue to the next mod.
                    if (!isDevelopment && !isEnabled && File.Exists(binBuildPath))
                        continue;

                    // Compute the path to the the mod assembly. If the path does not exist, continue to the next mod.
                    var relativeBuildPath = File.ReadAllText(binBuildPath);
                    var buildPath = System.IO.Path.Combine(folder, relativeBuildPath);

                    if (!File.Exists(buildPath))
                        continue;

                    // Load the mod's assembly file.
                    var assembly = Assembly.LoadFile(buildPath);

                    // Log the successfull load of the mod.
                    File.AppendAllText(System.IO.Path.Combine(folder, "mod.log"),
                        string.Format("[{0}] Info: Loaded {1}.\r\n", DateTime.Now.ToString("yy-MM-dd HH:mm:ss"),
                            assembly));

                    // Create an instance of the mod and register it in the mod manager.
                    var loadedAnyType = false;
                    foreach (var type in assembly.GetExportedTypes())
                    {
                        if (!typeof (IMod).IsAssignableFrom(type))
                            continue;

                        var userMod = Activator.CreateInstance(type) as IMod;

                        if (userMod == null)
                            continue;

                        // Bind additional mod information.
                        SetProperty(userMod, "Path", folder);
                        SetProperty(userMod, "Identifier", directoryName);

                        ModManager.Instance.addMod(userMod);
                        _loadedMods.Add(userMod);

                        // Enable mod if mods were already enabled
                        if (_isEnabled)
                            foreach (var modEntry in _modEntries)
                            {
                                if (modEntry.mod == userMod)
                                {
                                    modEntry.enableMod();
                                    break;
                                }
                            }

                        File.AppendAllText(System.IO.Path.Combine(folder, "mod.log"),
                            string.Format("[{0}] Info: Sucessfully registered {1} to the mod manager.\r\n",
                                DateTime.Now.ToString("yy-MM-dd HH:mm:ss"), userMod));

                        loadedAnyType = true;
                    }

                    if (!loadedAnyType)
                    {
                        File.AppendAllText(System.IO.Path.Combine(folder, "mod.log"),
                            string.Format("[{0}] Warn: No exported type in {1} implements IMod.\r\n",
                                DateTime.Now.ToString("yy-MM-dd HH:mm:ss"), assembly));
                    }
                }
                catch (Exception e)
                {
                    // Log failed loading attempts.
                    File.AppendAllText(System.IO.Path.Combine(folder, "mod.log"),
                        string.Format("[{0}] Fatal: Exception during loading: {1}.\r\n",
                            DateTime.Now.ToString("yy-MM-dd HH:mm:ss"), e.Message));
                }
            }
        }

        #region Implementation of IMod

        public void onEnabled()
        {
            _isEnabled = true;

            _gameObject = new GameObject();
            _gameObject.AddComponent<ModReloader>().ModLoader = this;
        }

        public void onDisabled()
        {
            _isEnabled = false;
        }

        public string Name
        {
            get { return "ParkitectNexus Mod Loader"; }
        }

        public string Description
        {
            get { return "A mod loader for mods distributed by ParkitectNexus.com."; }
        }

        #endregion
    }
}