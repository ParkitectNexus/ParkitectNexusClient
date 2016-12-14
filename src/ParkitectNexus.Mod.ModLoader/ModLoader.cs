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
using System.Globalization;
using System.IO;
using System.Reflection;
using MiniJSON;
using UnityEngine;
using FPath = System.IO.Path;

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
            var fieldInfo = typeof (ModManager).GetField("modEntries", BindingFlags.Instance | BindingFlags.NonPublic);
            _modEntries = fieldInfo.GetValue(ModManager.Instance) as List<ModManager.ModEntry>;

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
            {
                var setter = property.GetSetMethod() ?? property.GetSetMethod(true);
                if (setter != null)
                {
                    setter.Invoke(@object, BindingFlags.NonPublic | BindingFlags.Public, null, new object[] {value},
                                    CultureInfo.CurrentCulture);
                }
            }
        }

        private static T GetProperty<T>(object @object, string propertyName)
        {
            var property = @object.GetType ()
                .GetProperty (propertyName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null,
                    typeof (T), new Type [0], null);

            if (property == null)
                return default(T);

            var getter = property.GetGetMethod() ?? property.GetGetMethod(true);
            return getter == null ? default(T) : (T)getter.Invoke(@object, BindingFlags.NonPublic | BindingFlags.Public, null, null, CultureInfo.CurrentCulture);
        }

        private static void LogToMod(string folder, string level, string format, params object[] args)
        {
            File.AppendAllText(FPath.Combine(folder, "mod.log"),
                string.Format("[{0}] {1}: {2}\r\n", DateTime.Now.ToString("yy-MM-dd HH:mm:ss"), level,
                    string.Format(format, args)));
        }

        public void LoadMods()
        {
            // Unload mods loaded by this mod loader.
            foreach (var modEntry in _modEntries.ToArray())
            {
                if (!_loadedMods.Contains(modEntry.mod))
                    continue;

                if (_isEnabled)
                    modEntry.setActive(false);
                _modEntries.Remove(modEntry);
            }

            _loadedMods.Clear();

            // Compute paths to the mods directory.
            var modsPath = FilePaths.getFolderPath("pnmods");

            // Find mod directories in the mods directory.
            foreach (var directoryName in File.ReadAllLines(FPath.Combine(modsPath, "load.dat")))
            {
                if (string.IsNullOrEmpty(directoryName))
                    continue;

                var folder = FPath.Combine(modsPath, directoryName);
                var filePath = FPath.Combine(folder, "mod.json");

                if (!File.Exists(filePath))
                    continue;

                try
                {
                    // Read the mod.json file.
                    var dictionary = Json.Deserialize(File.ReadAllText(filePath)) as Dictionary<string, object>;

                    if (dictionary == null)
                        continue;

                    var isEnabled = ReadFromDictonary<bool>(dictionary, "IsEnabled");
                    var isDevelopment = ReadFromDictonary<bool>(dictionary, "IsDevelopment");
                    var priority = ReadFromDictonary<double>(dictionary, "Priority");

                    var binBuildPath = FPath.Combine(folder, "bin/build.dat");

                    // If the mod is not enabled or in development, continue to the next mod.
                    if ((!isDevelopment && !isEnabled) || !File.Exists(binBuildPath))
                        continue;

                    // Compute the path to the the mod assembly. If the path does not exist, continue to the next mod.
                    var relativeBuildPath = File.ReadAllText(binBuildPath);
                    var buildPath = FPath.Combine(folder, relativeBuildPath);

                    if (!File.Exists(buildPath))
                        continue;

                    // Load the mod's assembly file.
                    var assembly = Assembly.LoadFile(buildPath);

                    // Log the successfull load of the mod.
                    File.AppendAllText(FPath.Combine(folder, "mod.log"),
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
                        if(String.IsNullOrEmpty(GetProperty<String>(userMod, "Name")))
                            SetProperty(userMod, "Name", directoryName.Split('@')[1]);

                        ModManager.Instance.addMod(userMod, (int) priority);
                        _loadedMods.Add(userMod);

                        // Enable mod if mods were already enabled
                        if (_isEnabled)
                            foreach (var modEntry in _modEntries)
                            {
                                if (modEntry.mod == userMod)
                                {
                                    modEntry.setActive(true);
                                    break;
                                }
                            }

                        LogToMod(folder, "Info", "Sucessfully registered {0} to the mod manager.", userMod);
                        loadedAnyType = true;
                    }

                    if (!loadedAnyType)
                        LogToMod(folder, "Warn", "No exported type in {0} implements IMod.", assembly);
                }
                catch (Exception e)
                {
                    // Log failed loading attempts.
                    LogToMod(folder, "Fatal", "Exception during loading: {0}.", e.Message);
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

        public string Identifier
        {
            get { return "ParkitectNexus@ModLoader"; }
        }

        #endregion
    }
}
