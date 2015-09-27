using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MiniJSON;
using UnityEngine;

namespace ParkitectNexus.Mod.ModLoader
{
    public static class Main
    {
        public static void Load()
        {
            // Unload mods that are currently loaded. Because unloading mods is not yet publically available we need to
            // gain access to the mod entries list trough reflection.
            var field = typeof(ModManager).GetField("modEntries", BindingFlags.NonPublic | BindingFlags.Instance);
            var activeMods = field == null ? null : field.GetValue(ModManager.Instance) as List<ModManager.ModEntry>;
            
            var anyModActive = false;
            if (activeMods != null)
            {
                // If any mod is currently active we'll be activating the mods that will be loaded.
                anyModActive = activeMods.Any(m=> m.active);

                // Disable all active mod entries and clear the list.
                if (anyModActive)
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

                    object isEnabled, isDevelopment, entryPoint, developmentBuildPath;
                    
                    dictionary.TryGetValue("IsEnabled", out isEnabled);
                    dictionary.TryGetValue("IsDevelopment", out isDevelopment);
                    dictionary.TryGetValue("EntryPoint", out entryPoint);
                    dictionary.TryGetValue("DevelopmentBuildPath", out developmentBuildPath);
                    
                    bool bIsEnabled = (isEnabled is bool) ? (bool) isEnabled : false,
                        bisDevelopment = (isDevelopment is bool) ? (bool) isDevelopment : false;
                    string sEntryPoint = entryPoint as string,
                        sDevelopmentBuildPath = developmentBuildPath as string;

                    // If the mod is not enabled or in development, continue to the next mod.
                    if (!bisDevelopment && !bIsEnabled) continue;

                    // Decide the path to the the mod assembly. If the path does not exist, continue to the next mod.
                    var compiledPath = bisDevelopment
                        ? System.IO.Path.Combine(folder, sDevelopmentBuildPath)
                        : System.IO.Path.Combine(folder, "compiled.dll");
                    
                    if (!File.Exists(compiledPath)) continue;
                    
                    // Load the mod's assembly file.
                    var assembly = Assembly.LoadFile(compiledPath);
                    
                    // Log the successfull load of the mod.
                    File.AppendAllText(System.IO.Path.Combine(folder, "mod.log"),
                        string.Format("[{0}] Notice: Loaded {1}.\n", DateTime.Now.ToString("G"), assembly));
                    
                    // Create an instance of the mod and register it in the mod manager.
                    var userMod = assembly.CreateInstance(sEntryPoint) as IMod;
                    if (userMod == null) continue;

                    ModManager.Instance.addMod(userMod);
                }
                catch (Exception e)
                {
                    // Log failed loading attempts.
                    File.AppendAllText(System.IO.Path.Combine(folder, "mod.log"),
                        string.Format("[{0}] Fatal: Exception during loading: {1}.\n", DateTime.Now.ToString("G"),
                            e.Message));
                }
            }

            // If any mods were active before, re enable all newly loaded mods.
            if(anyModActive)
                ModManager.Instance.triggerEnable();
        }
    }
}