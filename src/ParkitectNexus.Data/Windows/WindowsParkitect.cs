// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.Data.Properties;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Windows
{
    /// <summary>
    ///     Represents the Parkitect game.
    /// </summary>
    public class WindowsParkitect : IParkitect
    {
        public WindowsParkitect()
        {
            Paths = new WindowsParkitectPaths(this);
        }

        /// <summary>
        ///     Gets or sets the installation path.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if the installation path is invalid</exception>
        public string InstallationPath
        {
            get
            {
                return IsValidInstallationPath(Settings.Default.InstallationPath)
                    ? Settings.Default.InstallationPath
                    : null;
            }
            set
            {
                if (!IsValidInstallationPath(value))
                    throw new ArgumentException("invalid installation path", nameof(value));

                Settings.Default.InstallationPath = value;
                Settings.Default.Save();
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the game is installed.
        /// </summary>
        public bool IsInstalled => IsValidInstallationPath(InstallationPath);

        /// <summary>
        ///     Gets a collection of paths.
        /// </summary>
        public IParkitectPaths Paths { get; }

        /// <summary>
        ///     Gets a collection of assembly names provided by the game.
        /// </summary>
        public IEnumerable<string> ManagedAssemblyNames
            =>
                !IsInstalled
                    ? null
                    : Directory.GetFiles(Paths.DataManaged, "*.dll").Select(Path.GetFileName);

        /// <summary>
        ///     Gets a collection of installed mods.
        /// </summary>
        public IEnumerable<IParkitectMod> InstalledMods
        {
            get
            {
                if (!IsInstalled)
                    yield break;

                // Iterate trough every directory in the mods directory which has a mod.json file.
                foreach (
                    var path in
                        Directory.GetDirectories(Paths.Mods).Where(path => File.Exists(Path.Combine(path, "mod.json"))))
                {
                    // Attempt to deserialize the mod.json file.
                    var mod = new ParkitectMod(this);
                    try
                    {
                        JsonConvert.PopulateObject(File.ReadAllText(Path.Combine(path, "mod.json")), mod);
                        mod.InstallationPath = path;
                    }
                    catch
                    {
                        mod = null;
                    }

                    // If the mod.json file was deserialized successfully, return the mod.
                    if (mod != null)
                        yield return mod;
                }
            }
        }

        /// <summary>
        ///     Sets the installation path if the specified path is a valid installation path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>true if valid; false otherwise.</returns>
        public bool SetInstallationPathIfValid(string path)
        {
            if (IsValidInstallationPath(path))
            {
                InstallationPath = path;
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Detects the installation path.
        /// </summary>
        /// <returns>true if the installation path has been detected; false otherwise.</returns>
        public bool DetectInstallationPath()
        {
            if (IsInstalled)
                return true;

            // TODO Detect registry key of installation path.
            // can only do this once it's installed trough steam or a setup.
            return false;
        }

        /// <summary>
        ///     Launches the game with the specified arguments.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The launched process.</returns>
        public Process Launch(string arguments = "-single-instance")
        {
            Log.WriteLine($"Attempting to launch game with arguments '{arguments}'.");

            Log.WriteLine("Attempting to compile installed mods.");
            CompileActiveMods();

            // If the process is already running, push it to the foreground and return it.
            var running = Process.GetProcessesByName("Parkitect").FirstOrDefault();

            if (running != null)
            {
                Log.WriteLine(
                    $"'Parkitect' is already running. Giving window handle '{running.MainWindowHandle}' focus.");

                User32.SetForegroundWindow(running.MainWindowHandle);
                return running;
            }

            Log.WriteLine($"Launching game at path '{Paths.Executable}'.");
            // Start the game process.
            return !IsInstalled
                ? null
                : Process.Start(new ProcessStartInfo(Paths.Executable)
                {
                    WorkingDirectory = InstallationPath,
                    Arguments = arguments
                });
        }

        /// <summary>
        ///     Stores the specified asset in the game's correct directory.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <returns>A task which performs the requested action.</returns>
        public async Task StoreAsset(IParkitectAsset asset)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));

            if (!IsInstalled)
                throw new Exception("parkitect is not installed");

            Log.WriteLine($"Storing asset {asset}.");

            // Gather information about the asset type.
            var assetInfo = asset.Type.GetCustomAttribute<ParkitectAssetInfoAttribute>();

            if (assetInfo == null)
                throw new Exception("invalid asset type");

            switch (asset.Type)
            {
                case ParkitectAssetType.Blueprint:
                case ParkitectAssetType.Savegame:
                    // Create the directory where the asset should be stored and create a path to where the asset should be stored.
                    var storagePath = Path.Combine(InstallationPath, assetInfo.StorageFolder);
                    var assetPath = Path.Combine(storagePath, asset.FileName);

                    Directory.CreateDirectory(storagePath);

                    Log.WriteLine($"Storing asset to {assetPath}.");

                    // If the file already exists, add a number behind the file name.
                    if (File.Exists(assetPath))
                    {
                        Log.WriteLine("Asset already exists, comparing hashes.");

                        var md5 = MD5.Create();

                        // Compute hash of downloaded asset to match with installed hash.
                        asset.Stream.Seek(0, SeekOrigin.Begin);
                        var validHash = md5.ComputeHash(asset.Stream);

                        if (validHash.SequenceEqual(md5.ComputeHash(File.OpenRead(assetPath))))
                        {
                            Log.WriteLine("Asset hashes match, aborting.");
                            return;
                        }

                        Log.WriteLine("Asset hashes mismatch, computing new file name.");
                        // Separate the filename and the extension.
                        var attempt = 1;
                        var fileName = Path.GetFileNameWithoutExtension(asset.FileName);
                        var fileExtension = Path.GetExtension(asset.FileName);

                        // Update the path to where the the asset should be stored by adding a number behind the name until an available filename has been found.
                        do
                        {
                            assetPath = Path.Combine(storagePath, $"{fileName} ({++attempt}){fileExtension}");

                            if (File.Exists(assetPath) &&
                                validHash.SequenceEqual(md5.ComputeHash(File.OpenRead(assetPath))))
                                return;
                        } while (File.Exists(assetPath));

                        Log.WriteLine($"Newly computed path is {assetPath}.");
                    }

                    Log.WriteLine("Writing asset to file.");
                    // Write the stream to a file at the asset path.
                    using (var fileStream = File.Create(assetPath))
                    {
                        asset.Stream.Seek(0, SeekOrigin.Begin);
                        await asset.Stream.CopyToAsync(fileStream);
                    }
                    break;
                case ParkitectAssetType.Mod:

                    Log.WriteLine("Attempting to open mod stream.");
                    using (var zip = new ZipArchive(asset.Stream, ZipArchiveMode.Read))
                    {
                        // Compute name of main directory inside archive.
                        var mainFolder = zip.Entries.FirstOrDefault()?.FullName;
                        if (mainFolder == null)
                            throw new Exception("invalid archive");

                        Log.WriteLine($"Mod archive main folder is {mainFolder}.");

                        // Find the mod.json file. Yay for / \ path divider compatibility.
                        var modJsonPath = Path.Combine(mainFolder, "mod.json").Replace('/', '\\');
                        var modJson = zip.Entries.FirstOrDefault(e => e.FullName.Replace('/', '\\') == modJsonPath);

                        // Read mod.json.
                        if (modJson == null) throw new Exception("mod is missing mod.json file");
                        using (var streamReader = new StreamReader(modJson.Open()))
                        {
                            var json = await streamReader.ReadToEndAsync();
                            var mod = new ParkitectMod(this);
                            JsonConvert.PopulateObject(json, mod);

                            Log.WriteLine($"mod.json was deserialized to mod object '{mod}'.");

                            // Set default mod properties.
                            mod.Tag = asset.DownloadInfo.Tag;
                            mod.Repository = asset.DownloadInfo.Repository;
                            mod.InstallationPath = Path.Combine(Paths.Mods,
                                asset.DownloadInfo.Repository.Replace('/', '@'));
                            mod.IsEnabled = true;
                            mod.IsDevelopment = false;

                            // Find previous version of mod.
                            var oldMod = InstalledMods.FirstOrDefault(m => m.Repository == mod.Repository);
                            if (oldMod != null)
                            {
                                Log.WriteLine("An installed version of this mod was detected.");
                                // This version was already installed.
                                if (oldMod.IsDevelopment || oldMod.Tag == mod.Tag)
                                {
                                    Log.WriteLine("Installed version is already up to date. Aborting.");
                                    return;
                                }

                                Log.WriteLine("Deleting installed version.");
                                oldMod.Delete();

                                // Deleting is stupid.
                                // TODO look for better solution
                                await Task.Delay(1000);
                            }

                            // Install mod.
                            Log.WriteLine("Copying mod to mods folder.");
                            foreach (var entry in zip.Entries)
                            {
                                if (!entry.FullName.StartsWith(mainFolder))
                                    continue;

                                // Compute path.
                                var partDir = entry.FullName.Substring(mainFolder.Length);
                                var path = Path.Combine(mod.InstallationPath, partDir);

                                if (string.IsNullOrEmpty(entry.Name))
                                {
                                    Log.WriteLine($"Creating directory '{path}'.");
                                    Directory.CreateDirectory(path);
                                }
                                else
                                {
                                    Log.WriteLine($"Storing mod file '{path}'.");
                                    using (var openStream = entry.Open())
                                    using (var fileStream = File.OpenWrite(path))
                                        await openStream.CopyToAsync(fileStream);
                                }
                            }

                            // Save and compile the mod.
                            mod.Save();
                            mod.CopyAssetBundles();
                            mod.Compile();
                        }
                    }
                    break;
                default:
                    throw new Exception("unsupported asset type");
            }
        }

        private static bool IsValidInstallationPath(string path)
        {
            // Path must exist and contain Parkitect.exe.
            return !string.IsNullOrWhiteSpace(path) && File.Exists(Path.Combine(path, "Parkitect.exe"));
        }

        private void CompileActiveMods()
        {
            foreach (var mod in InstalledMods)
            {
                if (mod.IsEnabled || mod.IsDevelopment)
                {
                    mod.CopyAssetBundles();
                    mod.Compile();
                }
            }
        }
    }
}