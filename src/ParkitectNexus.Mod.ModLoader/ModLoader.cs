using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using MiniJSON;

namespace ParkitectNexus.Mod.ModLoader
{
    public class ModLoader : IMod
    {
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

        public ModLoader()
        {
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
                    if (string.IsNullOrEmpty(entryPoint))
                        throw new Exception("No EntryPoint has been specified in the mod.json file");
                    
                    var modObject = assembly.CreateInstance(entryPoint);

                    if (modObject == null)
                        throw new Exception("The class specified as EntryPoint(" + entryPoint + ") failed to initialize (may not exist?)");

                    var userMod = modObject as IMod;
                    if (userMod == null)
                        throw new Exception("The class specified as EntryPoint(" + entryPoint + ") does not implement `IMod`");

                    // Bind additional mod information.
                    SetProperty(userMod, "Path", folder);
                    SetProperty(userMod, "Identifier", directoryName);

                    ModManager.Instance.addMod(userMod);

                    File.AppendAllText(System.IO.Path.Combine(folder, "mod.log"),
                        string.Format("[{0}] Info: Sucessfully registered {1} to the mod manager.\r\n",
                            DateTime.Now.ToString("yy-MM-dd HH:mm:ss"), userMod));
                }
                catch (Exception e)
                {
                    // Log failed loading attempts.
                    File.AppendAllText(System.IO.Path.Combine(folder, "mod.log"),
                        string.Format("[{0}] Fatal: Exception during loading: {1}.\r\n", DateTime.Now.ToString("G"),
                            e.Message));
                }
            }
        }

        public void onEnabled()
        {
            //
        }

        public void onDisabled()
        {
            //
        }

        public string Name
        {
            get { return "ParkitectNexus Mod Loader"; }
        }
        public string Description
        {
            get { return "A mod loader for mods distributed by ParkitectNexus.com."; }
        }
    }
}