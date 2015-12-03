// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using Microsoft.CSharp;
using Newtonsoft.Json;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data
{
    /// <summary>
    ///     Represents a Parkitect mod provided by ParkitectNexus.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ParkitectMod : IParkitectMod
    {
        /// <summary>
        ///     Assemblies provided by the mono runtime.
        /// </summary>
        private static readonly string[] SystemAssemblies =
        {
            "System", "System.Core", "System.Data", "System.Xml",
            "System.Xml.Linq", "Microsoft.CSharp", "System.Data.DataSetExtensions", "System.Net.Http"
        };

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public ParkitectMod(IParkitect parkitect)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            Parkitect = parkitect;
        }

        /// <summary>
        ///     Opens the log for this mod instance.
        /// </summary>
        /// <returns>A stream writer for mod related logging.</returns>
        public StreamWriter OpenLog()
        {
            return !IsInstalled ? null : File.AppendText(Path.Combine(InstallationPath, "mod.log"));
        }

        /// <summary>
        ///     Saves this instance.
        /// </summary>
        public void Save()
        {
            if (!IsInstalled)
                throw new Exception("Not installed");

            Log.WriteLine("Saving mod configuration.");
            File.WriteAllText(Path.Combine(InstallationPath, "mod.json"), JsonConvert.SerializeObject(this));
        }

        /// <summary>
        ///     Deletes this instance.
        /// </summary>
        public void Delete()
        {
            if (!IsInstalled) throw new Exception("mod not installed");
            if (AssetBundlePrefix == null)
                return;

            Log.WriteLine($"Deleting mod '{this}'.");

            var modAssetBundlePath = Path.Combine(Parkitect.Paths.Data, "StreamingAssets/mods", AssetBundlePrefix);

            Directory.Delete(InstallationPath, true);

            if (Directory.Exists(modAssetBundlePath))
                Directory.Delete(modAssetBundlePath, true);

            InstallationPath = null;
        }

        /// <summary>
        ///     Compiles this instance.
        /// </summary>
        /// <returns>true on success; false otherwise.</returns>
        public bool Compile()
        {
            if (!IsInstalled) throw new Exception("mod not installed");

            Log.WriteLine($"Compiling mod '{this}'.");

            using (var logFile = OpenLog())
            {
                try
                {
                    // Delete old builds.
                    var binPath = Path.Combine(InstallationPath, "bin");
                    if (Directory.Exists(binPath))
                    {
                        foreach (var build in Directory.GetFiles(binPath, "build-*.dll"))
                            try
                            {
                                File.Delete(build);
                            }
                            catch
                            {
                            }
                    }

                    var buildPath = GetBuildPath();

                    // Compute build path
                    if (IsDevelopment || string.IsNullOrWhiteSpace(buildPath) ||
                        !File.Exists(Path.Combine(InstallationPath, buildPath)))
                    {
                        Directory.CreateDirectory(binPath);
                        buildPath = $"bin/build-{DateTime.Now.ToString("yyMMddHHmmssfff")}.dll";
                        SetBuildPath(buildPath);
                    }

                    // Delete existing compiled file if compilation is forced.
                    if (File.Exists(Path.Combine(InstallationPath, buildPath)))
                    {
                        return true;
                    }

                    logFile.Log($"Compiling {Name} to {buildPath}...");
                    Log.WriteLine($"Compiling {Name} to {buildPath}...");

                    var assemblyFiles = new List<string>();
                    var sourceFiles = new List<string>();

                    var codeDir = string.IsNullOrWhiteSpace(BaseDir) || BaseDir.All(c => c == '/' || c == '\\')
                        ? InstallationPath
                        : Path.Combine(InstallationPath, BaseDir);

                    var csProjPath = Project == null ? null : Path.Combine(codeDir, Project);

                    List<string> unresolvedAssemblyReferences;
                    List<string> unresolvedSourceFiles;

                    if (csProjPath != null)
                    {
                        // Load source files and referenced assemblies from *.csproj file.
                        logFile.Log($"Compiling from `{Project}`.");
                        Log.WriteLine($"Compiling from `{Project}`.");

                        // Open the .csproj file of the mod.
                        var document = new XmlDocument();
                        document.Load(csProjPath);

                        var manager = new XmlNamespaceManager(document.NameTable);
                        manager.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

                        // List the referenced assemblies of the mod.
                        unresolvedAssemblyReferences = document.SelectNodes("//x:Reference", manager)
                            .Cast<XmlNode>()
                            .Select(node => node.Attributes["Include"])
                            .Select(name => name.Value.Split(',').FirstOrDefault()).ToList();

                        // List the source files of the mod.
                        unresolvedSourceFiles = document.SelectNodes("//x:Compile", manager)
                            .Cast<XmlNode>()
                            .Select(node => node.Attributes["Include"].Value).ToList();
                    }
                    else
                    {
                        // Load source files and referenced assemblies from mod.json file.
                        logFile.Log("Compiling from `mod.json`.");
                        Log.WriteLine("Compiling from `mod.json`.");

                        unresolvedAssemblyReferences = ReferencedAssemblies.ToList();
                        unresolvedSourceFiles = CodeFiles.ToList();
                    }

                    // Resolve the assembly references.
                    foreach (var name in unresolvedAssemblyReferences)
                    {
                        var resolved = ResolveAssembly(name);
                        assemblyFiles.Add(resolved);

                        logFile.Log($"Resolved assembly reference `{name}` to `{resolved}`");
                        Log.WriteLine($"Resolved assembly reference `{name}` to `{resolved}`");
                    }

                    // Resolve the source file paths.
                    logFile.Log($"Source files: {string.Join(", ", unresolvedSourceFiles)} from `{codeDir}`.");
                    Log.WriteLine($"Source files: {string.Join(", ", unresolvedSourceFiles)} from `{codeDir}`.");
                    sourceFiles.AddRange(
                        unresolvedSourceFiles.Select(file => {
                            var repl = file.Replace("\\", Path.DirectorySeparatorChar.ToString());
                            return Path.Combine(codeDir, repl);
                        }));

                    // Compile.
                    var csCodeProvider =
                        new CSharpCodeProvider(new Dictionary<string, string> {{"CompilerVersion", CompilerVersion}});
                    var parameters = new CompilerParameters(assemblyFiles.ToArray(),
                        Path.Combine(InstallationPath, buildPath));

                    var result = csCodeProvider.CompileAssemblyFromFile(parameters, sourceFiles.ToArray());

                    // Log errors.
                    foreach (var error in result.Errors.Cast<CompilerError>())
                    {
                        logFile.Log(
                            $"{error.ErrorNumber}: {error.Line}:{error.Column}: {error.ErrorText} in {error.FileName}",
                            LogLevel.Error);
                        Log.WriteLine(
                            $"{error.ErrorNumber}: {error.Line}:{error.Column}: {error.ErrorText} in {error.FileName}",
                            LogLevel.Error);
                    }
                    return !result.Errors.HasErrors;
                }
                catch (Exception e)
                {
                    logFile.Log(e.Message, LogLevel.Error);
                    Log.WriteLine(e.Message, LogLevel.Error);
                    return false;
                }
            }
        }

        /// <summary>
        ///     Copies the asset bundles to the games assets directory.
        /// </summary>
        /// <returns>true on success; false otherwise.</returns>
        public bool CopyAssetBundles()
        {
            if (!IsInstalled) throw new Exception("mod not installed");
            if (AssetBundleDir == null) return true;
            if (AssetBundlePrefix == null)
                throw new Exception("AssetBundlePrefix is required when an AssetBundleDir is set");

            Log.WriteLine("Copying asset bundle.");

            using (var logFile = OpenLog())
            {
                try
                {
                    var modAssetBundlePath = Path.Combine(Parkitect.Paths.Data, "StreamingAssets/mods",
                        AssetBundlePrefix);
                    var files = Directory.GetFiles(Path.Combine(InstallationPath, AssetBundleDir));
                    var fileNames = files.Select(Path.GetFileName).ToArray();
                    var md5 = MD5.Create();

                    Directory.CreateDirectory(modAssetBundlePath);

                    // Delete old files no longer required for mod.
                    foreach (var oldFile in Directory.GetFiles(modAssetBundlePath)
                        .Where(f => !fileNames.Contains(Path.GetFileName(f))))
                        File.Delete(oldFile);

                    foreach (var assetBundleFile in Directory.GetFiles(Path.Combine(InstallationPath, AssetBundleDir)))
                    {
                        var targetPath = Path.Combine(modAssetBundlePath, Path.GetFileName(assetBundleFile));

                        // If file already exists in streaming directory, check if the hashes match.
                        if (File.Exists(targetPath))
                        {
                            try
                            {
                                byte[] oldHash,
                                    newHash;
                                using (var stream = File.OpenRead(targetPath))
                                    oldHash = md5.ComputeHash(stream);
                                using (var stream = File.OpenRead(assetBundleFile))
                                    newHash = md5.ComputeHash(stream);

                                if (oldHash.SequenceEqual(newHash))
                                    continue;
                            }
                            catch
                            {
                                continue;
                            }
                        }

                        File.Copy(assetBundleFile, targetPath, true);
                    }
                }
                catch (Exception e)
                {
                    logFile.Log(e.Message, LogLevel.Error);
                    Log.WriteLine(e.Message, LogLevel.Error);
                    return false;
                }
            }

            return true;
        }

        #region Overrides of Object

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion

        private string ResolveAssembly(string assemblyName)
        {
            if (assemblyName == null) throw new ArgumentNullException(nameof(assemblyName));

            var dllName = $"{assemblyName}.dll";

            if (SystemAssemblies.Contains(assemblyName))
                return dllName;
            
            var modPath = Path.Combine(InstallationPath, BaseDir ?? "", dllName);
            if (File.Exists(Path.Combine(modPath)))
                return modPath;

            var man = Parkitect.ManagedAssemblyNames.ToArray();
            if (man.Contains(dllName))
                return Path.Combine(Parkitect.Paths.DataManaged, dllName);

            throw new Exception($"Failed to resolve referenced assembly '{assemblyName}'");
        }

        private string GetBuildPath()
        {
            var currentFile = Path.Combine(InstallationPath, "bin/build.dat");

            if (!File.Exists(currentFile))
                return null;

            var relativePath = File.ReadAllText(currentFile);
            return File.Exists(Path.Combine(InstallationPath, relativePath)) ? relativePath : null;
        }

        private void SetBuildPath(string relativePath)
        {
            var currentFile = Path.Combine(InstallationPath, "bin/build.dat");
            File.WriteAllText(currentFile, relativePath);
        }

        #region Properties of ParkitectMod

        /// <summary>
        ///     Gets the parkitect instance this mod was installed to.
        /// </summary>
        public IParkitect Parkitect { get; }

        /// <summary>
        ///     Gets or sets the asset bundle directory.
        /// </summary>
        [JsonProperty]
        public string AssetBundleDir { get; set; }

        /// <summary>
        ///     Gets the asset bundle prefix.
        /// </summary>
        public string AssetBundlePrefix => !IsInstalled ? null : Path.GetFileName(InstallationPath);

        /// <summary>
        ///     Gets or sets the base directory.
        /// </summary>
        [JsonProperty]
        public string BaseDir { get; set; }

        /// <summary>
        ///     Gets or sets the compiler version.
        /// </summary>
        [JsonProperty]
        public string CompilerVersion { get; set; } = "v3.5";

        /// <summary>
        ///     Gets or sets the code files.
        /// </summary>
        [JsonProperty]
        public IList<string> CodeFiles { get; set; } = new List<string>();

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is development.
        /// </summary>
        [JsonProperty]
        public bool IsDevelopment { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        [JsonProperty]
        public bool IsEnabled { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is installed.
        /// </summary>
        public bool IsInstalled
            => !string.IsNullOrWhiteSpace(InstallationPath) && File.Exists(Path.Combine(InstallationPath, "mod.json"));

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the installation path.
        /// </summary>
        public string InstallationPath { get; set; }

        /// <summary>
        ///     Gets or sets the project.
        /// </summary>
        [JsonProperty]
        public string Project { get; set; }

        /// <summary>
        ///     Gets or sets the tag.
        /// </summary>
        [JsonProperty]
        public string Tag { get; set; }

        /// <summary>
        ///     Gets or sets the referenced assemblies.
        /// </summary>
        [JsonProperty]
        public IList<string> ReferencedAssemblies { get; set; } = new List<string>();

        /// <summary>
        ///     Gets or sets the repository.
        /// </summary>
        [JsonProperty]
        public string Repository { get; set; }

        #endregion
    }
}