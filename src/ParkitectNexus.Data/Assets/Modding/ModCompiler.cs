// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.CSharp;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Assets.Modding
{
    public class ModCompiler : IModCompiler
    {
        private readonly IParkitect _parkitect;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ModCompiler(IParkitect parkitect)
        {
            _parkitect = parkitect;
        }

        public async Task<ModCompileResults> Compile(IModAsset mod)
        {
            return await Task.Run(() =>
            {
                var dependencies = mod.Information.Dependencies?.Select(repository =>
                {
                    var dep =
                        _parkitect.Assets[AssetType.Mod].OfType<IModAsset>()
                            .FirstOrDefault(m => m.Repository == repository);

                    if (dep == null)
                        throw new Exception($"Dependency {repository} was not installed.");

                    return dep;
                }).ToArray() ?? new IModAsset[0];

                using (var logFile = mod.OpenLogFile())
                {
                    try
                    {
                        // Delete old builds.
                        var binPath = Path.Combine(mod.InstallationPath, "bin");
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

                        var buildPath = GetBuildPath(mod);

                        // Compute build path
                        if (mod.Information.IsDevelopment || string.IsNullOrWhiteSpace(buildPath) ||
                            !File.Exists(Path.Combine(mod.InstallationPath, buildPath)))
                        {
                            Directory.CreateDirectory(binPath);
                            buildPath = $"bin/build-{DateTime.Now.ToString("yyMMddHHmmssfff")}.dll";
                            SetBuildPath(mod, buildPath);
                        }

                        var fullBuildPath = Path.Combine(mod.InstallationPath, buildPath);

                        // Delete existing compiled file if compilation is forced.
                        if (File.Exists(fullBuildPath))
                        {
                            return ModCompileResults.Successful;
                        }

                        logFile.Log($"Compiling {mod.Name} to {buildPath}...");

                        var assemblyFiles = new List<string>();
                        var sourceFiles = new List<string>();

                        var codeDir = string.IsNullOrWhiteSpace(mod.Information.BaseDir) ||
                                      mod.Information.BaseDir.All(c => c == '/' || c == '\\')
                            ? mod.InstallationPath
                            : Path.Combine(mod.InstallationPath, mod.Information.BaseDir);

                        var csProjPath = mod.Information.Project == null
                            ? null
                            : Path.Combine(codeDir, mod.Information.Project);

                        List<string> unresolvedAssemblyReferences;
                        List<string> unresolvedSourceFiles;

                        if (csProjPath != null)
                        {
                            // Load source files and referenced assemblies from *.csproj file.
                            logFile.Log($"Compiling from `{mod.Information.Project}`.");

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
                            throw new Exception("No project file set");
                        }

                        // Resolve the assembly references.
                        foreach (var name in unresolvedAssemblyReferences)
                        {
                            var resolved = ResolveAssembly(dependencies, name);

                            if (resolved != null)
                            {
                                assemblyFiles.Add(resolved);

                                logFile.Log($"Resolved assembly reference `{name}` to `{resolved}`");
                            }
                            else
                            {
                                logFile.Log($"IGNORING assembly reference `{name}`");
                            }
                        }

                        foreach (var depMod in dependencies)
                        {
                            var dep = GetBuildPath(depMod);
                            if (dep == null)
                                throw new Exception($"Dependency {depMod.Name} wasn't build yet");
                            assemblyFiles.Add(Path.Combine(depMod.InstallationPath, dep));
                        }

                        // Resolve the source file paths.
                        logFile.Log($"Source files: {string.Join(", ", unresolvedSourceFiles)} from `{codeDir}`.");
                        sourceFiles.AddRange(
                            unresolvedSourceFiles.Select(file =>
                            {
                                var repl = file.Replace("\\", Path.DirectorySeparatorChar.ToString());
                                return Path.Combine(codeDir, repl);
                            }));

                        // Compile.
                        logFile.Log($"Compile using compiler version {mod.Information.CompilerVersion ?? "v3.5"}.");
                        var csCodeProvider =
                            new CSharpCodeProvider(new Dictionary<string, string>
                            {
                                {"CompilerVersion", mod.Information.CompilerVersion ?? "v3.5"}
                            });
                        var parameters = new CompilerParameters(assemblyFiles.ToArray(), fullBuildPath);

                        var result = csCodeProvider.CompileAssemblyFromFile(parameters, sourceFiles.ToArray());

                        // Copy to persistant instance
                        if (File.Exists(fullBuildPath))
                        {
                            var persistantPath = Path.Combine(mod.InstallationPath,
                                $"bin/{Path.GetFileName(mod.InstallationPath)}.dll");

                            try
                            {
                                File.Copy(fullBuildPath, persistantPath, true);
                            }
                            catch (Exception e)
                            {
                                logFile.Log($"Could not copy binaries to persistant path {persistantPath}!",
                                    LogLevel.Warn);
                                logFile.Log(e.Message, LogLevel.Warn);
                            }
                        }

                        // Log errors.
                        foreach (var error in result.Errors.Cast<CompilerError>())
                        {
                            logFile.Log(
                                $"{error.ErrorNumber}: {error.Line}:{error.Column}: {error.ErrorText} in {error.FileName}",
                                LogLevel.Error);
                        }

                        return result.Errors.HasErrors
                            ? new ModCompileResults(result.Errors.OfType<CompilerError>().ToArray(), false)
                            : ModCompileResults.Successful;
                    }
                    catch (Exception e)
                    {
                        logFile.Log(e.Message, LogLevel.Error);
                        return ModCompileResults.Failure;
                    }
                }
            });
        }

        private string GetBuildPath(IModAsset mod)
        {
            var currentFile = Path.Combine(mod.InstallationPath, "bin/build.dat");

            if (!File.Exists(currentFile))
                return null;

            var relativePath = File.ReadAllText(currentFile);
            return File.Exists(Path.Combine(mod.InstallationPath, relativePath)) ? relativePath : null;
        }

        private void SetBuildPath(IModAsset mod, string relativePath)
        {
            var currentFile = Path.Combine(mod.InstallationPath, "bin/build.dat");
            File.WriteAllText(currentFile, relativePath);
        }

        private string ResolveAssembly(IModAsset[] dependencies, string assemblyName)
        {
            if (assemblyName == null) throw new ArgumentNullException(nameof(assemblyName));

            var dllName = $"{assemblyName}.dll";

//            if (SystemAssemblies.Contains(assemblyName))
//                return dllName;

            if (IgnoredAssemblies.Contains(assemblyName))
                return null;

//            var modPath = Path.Combine(InstallationPath, BaseDir ?? "", dllName);
//            if (File.Exists(Path.Combine(modPath)))
//                return modPath;

            var managedAssemblyNames =
                Directory.GetFiles(_parkitect.Paths.DataManaged, "*.dll").Select(Path.GetFileName).ToArray();

            if (managedAssemblyNames.Contains(dllName))
                return Path.Combine(_parkitect.Paths.DataManaged, dllName);

            if (SystemAssemblies.Contains(assemblyName))
                return dllName;

            return null;
            //throw new Exception($"Failed to resolve referenced assembly '{assemblyName}'");
        }

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
    }
}
