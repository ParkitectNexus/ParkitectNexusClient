using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MiniJSON;
using UnityEngine;

namespace ParkitectNexus.Mod.ModLoader
{
    public static class Main
    {
        private static T ReadFromDictonary<T>(IDictionary<string, object> dictionary, string key)
        {
            object o;
            return dictionary.TryGetValue(key, out o) ? (o is T ? (T) o : default(T)) : default(T);
        }

        public static void Load()
        {
            // Unload mods that are currently loaded. Because unloading mods is not yet publically available we need to
            // gain access to the mod entries list trough reflection.
            var field = typeof(ModManager).GetField("modEntries", BindingFlags.NonPublic | BindingFlags.Instance);
            var activeMods = field == null ? null : field.GetValue(ModManager.Instance) as List<ModManager.ModEntry>;
            
            if (activeMods != null)
            {
                // Disable all active mod entries and clear the list.
                ModManager.Instance.triggerDisable();
                activeMods.Clear();
            }

            // Compute paths to the mods directory.
            var modsPath = System.IO.Path.Combine(System.IO.Path.Combine(Application.dataPath, "../"), "mods");

            // Find mod directories in the mods directory.
            foreach (var folder in Directory.GetDirectories(modsPath))
            {
                try
                {
                    var filePath = System.IO.Path.Combine(folder, "mod.json");
                    
                    if (!File.Exists(filePath))
                        continue;

                    // Read the mod.json file.
                    var dictionary = Json.Deserialize(File.ReadAllText(filePath)) as Dictionary<string, object>;

                    if (dictionary == null)
                        continue;

                    var isEnabled = ReadFromDictonary<bool>(dictionary, "IsEnabled");
                    var isDevelopment = ReadFromDictonary<bool>(dictionary, "IsDevelopment");
                    var entryPoint = ReadFromDictonary<string>(dictionary, "EntryPoint");

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
                        string.Format("[{0}] Info: Loaded {1}.\r\n", DateTime.Now.ToString("G"), assembly));
                    
                    // Create an instance of the mod and register it in the mod manager.
                    if(string.IsNullOrEmpty(entryPoint))
                        throw new Exception("No EntryPoint has been specified in the mod.json file");
                    
                    var userMod = assembly.CreateInstance(entryPoint) as IMod;
                    if (userMod == null)
                        throw new Exception("The class specified as EntryPoint does not implement IUserMod");

                    ModManager.Instance.addMod(userMod);

                    File.AppendAllText(System.IO.Path.Combine(folder, "mod.log"),
                        string.Format("[{0}] Info: Sucessfully registered {1} to the mod manager.\n",
                            DateTime.Now.ToString("G"), userMod));
                }
                catch (Exception e)
                {
                    // Log failed loading attempts.
                    File.AppendAllText(System.IO.Path.Combine(folder, "mod.log"),
                        string.Format("[{0}] Fatal: Exception during loading: {1}.\r\n", DateTime.Now.ToString("G"),
                            e.Message));
                }
            }

            // If the game is being played, enable mods.
            if(Application.loadedLevel == 2)
                ModManager.Instance.triggerEnable();
            
        }
    }
}