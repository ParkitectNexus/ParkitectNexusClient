// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.CSharp;
using Newtonsoft.Json;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Game
{
    /// <summary>
    ///     Represents a Parkitect mod provided by ParkitectNexus.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ParkitectMod :IParkitectMod
    {
        /// <summary>
        ///     Assemblies provided by the mono runtime.
        /// </summary>
        private static readonly string[] SystemAssemblies =
        {
            "System", "System.Core", "System.Data", "System.Xml",
            "System.Xml.Linq", "System.Data.DataSetExtensions", "System.Net.Http"
        };

        /// <summary>
        ///     References ignored during compilation.
        /// </summary>
        private static readonly string[] IgnoredAssemblies =
        {
            "Microsoft.CSharp"
        };

        private readonly ILogger _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParkitectMod"/> class.
        /// </summary>
        /// <param name="parkitect">The parkitect.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">parkitect or logger is null.</exception>
        public ParkitectMod(IParkitect parkitect, ILogger logger)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
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

            _logger.WriteLine("Saving mod configuration.");
            File.WriteAllText(Path.Combine(InstallationPath, "mod.json"), JsonConvert.SerializeObject(this));
        }

        /// <summary>
        ///     Deletes this instance.
        /// </summary>
        public void Delete()
        {
            if (!IsInstalled) throw new Exception("mod not installed");

            _logger.WriteLine($"Deleting mod '{this}'.");

            Directory.Delete(InstallationPath, true);

            InstallationPath = null;
        }

        /// <summary>
        ///     Compiles this instance.
        /// </summary>
        /// <returns>true on success; false otherwise.</returns>
        public bool Compile()
        {
            if (!IsInstalled) throw new Exception("mod not installed");

            _logger.WriteLine($"Compiling mod '{this}'.");

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
                    _logger.WriteLine($"Compiling {Name} to {buildPath}...");

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
                        _logger.WriteLine($"Compiling from `{Project}`.");

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
                        _logger.WriteLine("Compiling from `mod.json`.");

                        unresolvedAssemblyReferences = ReferencedAssemblies.ToList();
                        unresolvedSourceFiles = CodeFiles.ToList();
                    }

                    // Resolve the assembly references.
                    foreach (var name in unresolvedAssemblyReferences)
                    {
                        var resolved = ResolveAssembly(name);

                        if (resolved != null)
                        {
                            assemblyFiles.Add(resolved);

                            logFile.Log($"Resolved assembly reference `{name}` to `{resolved}`");
                            _logger.WriteLine($"Resolved assembly reference `{name}` to `{resolved}`");
                        }
                        else
                        {
                            logFile.Log($"IGNORING assembly reference `{name}`");
                            _logger.WriteLine($"IGNORING assembly reference `{name}`");
                        }
                    }

                    // Resolve the source file paths.
                    logFile.Log($"Source files: {string.Join(", ", unresolvedSourceFiles)} from `{codeDir}`.");
                    _logger.WriteLine($"Source files: {string.Join(", ", unresolvedSourceFiles)} from `{codeDir}`.");
                    sourceFiles.AddRange(
                        unresolvedSourceFiles.Select(file =>
                        {
                            var repl = file.Replace("\\", Path.DirectorySeparatorChar.ToString());
                            return Path.Combine(codeDir, repl);
                        }));

                    // Compile.
                    var csCodeProvider =
                        new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", CompilerVersion } });
                    var parameters = new CompilerParameters(assemblyFiles.ToArray(),
                        Path.Combine(InstallationPath, buildPath));

                    var result = csCodeProvider.CompileAssemblyFromFile(parameters, sourceFiles.ToArray());

                    // Log errors.
                    foreach (var error in result.Errors.Cast<CompilerError>())
                    {
                        logFile.Log(
                            $"{error.ErrorNumber}: {error.Line}:{error.Column}: {error.ErrorText} in {error.FileName}",
                            LogLevel.Error);
                        _logger.WriteLine(
                            $"{error.ErrorNumber}: {error.Line}:{error.Column}: {error.ErrorText} in {error.FileName}",
                            LogLevel.Error);
                    }
                    return !result.Errors.HasErrors;
                }
                catch (Exception e)
                {
                    logFile.Log(e.Message, LogLevel.Error);
                    _logger.WriteLine(e.Message, LogLevel.Error);
                    return false;
                }
            }
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

            if (IgnoredAssemblies.Contains(assemblyName))
                return null;

            var modPath = Path.Combine(InstallationPath, BaseDir ?? "", dllName);
            if (File.Exists(Path.Combine(modPath)))
                return modPath;

            if (SystemAssemblies.Contains(assemblyName))
                return dllName;

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

        public Image Thumbnail { get; } = null;

        public Stream Open()
        {
            throw new NotImplementedException("Can't open a mod");
        }

        public AssetType Type { get; } = AssetType.Mod;

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
