// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Microsoft.CSharp;
using Newtonsoft.Json;
using ParkitectNexus.Data;
using ParkitectNexus.ModLoader;

namespace ParkitectNexus.ModLauncher
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parkitect = new Parkitect();

            // Debug
            if (!parkitect.IsInstalled)
                parkitect.SetInstallationPathIfValid(@"C:\Users\Tim\Desktop\Parkitect - Modded");

            if (!parkitect.IsInstalled)
            {
                if (!parkitect.SetInstallationPathIfValid(
                    Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)))
                {
                    Console.WriteLine("Couldn't detect parkitect");
                    Console.ReadLine();
                    //todo show error. ParkitectModLoader must be inside the parkitect game folder.
                    return;
                }
            }

            var modsDirectory = Path.Combine(parkitect.InstallationPath, "mods");
            Directory.CreateDirectory(modsDirectory);

            var modsFile = Path.Combine(modsDirectory, "mods.json");
            var parkitectExecutable = Path.Combine(parkitect.InstallationPath, "Parkitect.exe");

            if (!File.Exists(parkitectExecutable))
            {
                // no exe
                return;
            }
            if (!File.Exists(modsFile))
            {
                // no mods
                Process.Start(new ProcessStartInfo(parkitectExecutable) {WorkingDirectory = parkitect.InstallationPath, Arguments = "-single-instance" });
                return;
            }

            var loadableModDatas = new List<LoadableModData>();

            var mods = JsonConvert.DeserializeObject<ParkitectMod[]>(File.ReadAllText(modsFile));
            var managedData = Path.Combine(parkitect.InstallationPath, @"Parkitect_Data\Managed");
            foreach (var mod in mods)
            {
                Console.WriteLine($"{mod.Name}: {mod.NameSpace}.{mod.ClassName}");
                
                // Compute some paths
                var modPath = Path.Combine(modsDirectory, mod.FolderName);
                var modAssemblyPath = Path.Combine(modPath, mod.AssemblyFileName);

                // Delete existing compiled file if compilation is forced.
                if (File.Exists(modAssemblyPath) && mod.ForceCompile)
                    File.Delete(modAssemblyPath);

                // If mod was already compiled, use this compiled version.
                if (File.Exists(modAssemblyPath))
                {
                    loadableModDatas.Add(new LoadableModData {Path = modAssemblyPath, Mod = mod});
                }

                // If the mod has not yet been compiled, compile it.
                else
                {
                    // Compile files with default references.
                    var csCodeProvider = new CSharpCodeProvider(new Dictionary<string, string>() {{"CompilerVersion", "v4.0"}});
                    var parameters =
                        new CompilerParameters(
                            new[]
                            {
                                "UnityEngine.dll", "UnityEngine.UI.dll", "Assembly-CSharp.dll"
                            }.Select(n => Path.Combine(managedData, n))
                                .ToArray()
                                .Concat(new[]
                                {
                                    "mscorlib.dll", "System.dll", "System.Core.dll", "System.Data.dll"
                                })
                                .ToArray(),
                            modAssemblyPath);

                    var results = csCodeProvider.CompileAssemblyFromFile(parameters,
                        mod.CodeFiles.Select(n => Path.Combine(modPath, n)).ToArray());

                    results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
                    // todo log errors and warnings to log file in mod folder

                    if (!results.Errors.HasErrors)
                    {
                        loadableModDatas.Add(new LoadableModData {Path = modAssemblyPath, Mod = mod});
                    }
                }
            }

            Console.WriteLine("Start Parkitect...");
            var process = Process.Start(new ProcessStartInfo(parkitectExecutable) {WorkingDirectory = parkitect.InstallationPath, Arguments = "-single-instance"});

            Console.WriteLine("Waiting for game to start...");
            Thread.Sleep(1000);
            
            while (!process.HasExited && process.MainWindowTitle.Contains("Configuration"))
            {
                Thread.Sleep(500);
                process.Refresh();
            }

            if (process.HasExited)
                if (process.HasExited)
                {

                    Console.WriteLine("Process was closed.");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                    return;
                }
            
            foreach (var m in loadableModDatas)
            {
                Console.Write($"Loading mod {m.Mod.Name}...");
                ModInjector.Inject(m.Path, m.Mod.NameSpace, m.Mod.ClassName, "Load");
                Console.WriteLine("Done!");
            }


            Console.WriteLine("Done!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }

    internal struct LoadableModData
    {
        public string Path { get; set; }
        public ParkitectMod Mod { get; set; }
    }
}