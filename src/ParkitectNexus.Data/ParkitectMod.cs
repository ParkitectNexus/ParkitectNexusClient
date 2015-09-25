// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.CSharp;
using Newtonsoft.Json;

namespace ParkitectNexus.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ParkitectMod
    {
        private static readonly string[] SystemAssemblies =
        {
            "System", "System.Core", "System.Data", "System.Xml",
            "System.Xml.Linq", "Microsoft.CSharp", "System.Data.DataSetExtensions", "System.Net.Http"
        };

        public string AssemblyPath => !IsInstalled ? null : System.IO.Path.Combine(Path, "compiled.dll");

        [JsonProperty]
        public string BaseDir { get; set; }

        [JsonProperty]
        public string ClassName { get; set; } = "Main";

        [JsonProperty]
        public string CompilerVersion { get; set; } = "v4.0";

        [JsonProperty]
        public IList<string> CodeFiles { get; set; } = new List<string>();
        
        [JsonProperty]
        public bool IsDevelopment { get; set; }

        [JsonProperty]
        public bool IsEnabled { get; set; }

        public bool IsInstalled
            => !String.IsNullOrWhiteSpace(Path) && File.Exists(System.IO.Path.Combine(Path, "mod.json"));

        [JsonProperty]
        public string MethodName { get; set; } = "Load";

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string NameSpace { get; set; }

        public string Path { get; set; }

        [JsonProperty]
        public string Project { get; set; }

        [JsonProperty]
        public string Tag { get; set; }

        [JsonProperty]
        public IList<string> ReferencedAssemblies { get; set; } = new List<string>();

        [JsonProperty]
        public string Repository { get; set; }

        private string ResolveAssembly(Parkitect parkitect, string assemblyName)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (assemblyName == null) throw new ArgumentNullException(nameof(assemblyName));

            var dllName = $"{assemblyName}.dll";
            if (SystemAssemblies.Contains(assemblyName))
                return dllName;

            if (parkitect.ManagedAssemblyNames.Contains(dllName))
                return System.IO.Path.Combine(parkitect.ManagedDataPath, dllName);

            var modPath = System.IO.Path.Combine(Path, BaseDir ?? "", dllName);
            if (File.Exists(System.IO.Path.Combine(modPath)))
                return modPath;

            throw new Exception($"Failed to resolve referenced assembly '{assemblyName}'");
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

        public StreamWriter OpenLog()
        {
            return !IsInstalled ? null : File.AppendText(System.IO.Path.Combine(Path, BaseDir ?? "", "mod.log"));
        }

        public void Save()
        {
            if (!IsInstalled)
                throw new Exception("Not installed");

            File.WriteAllText(System.IO.Path.Combine(Path, "mod.json"), JsonConvert.SerializeObject(this));
        }

        public void Delete()
        {
            if (!IsInstalled) throw new Exception("mod not installed");
            DeleteFileSystemInfo(new DirectoryInfo(Path));
        }

        private static void DeleteFileSystemInfo(FileSystemInfo fileSystemInfo)
        {
            var directoryInfo = fileSystemInfo as DirectoryInfo;
            if (directoryInfo != null)
            {
                foreach (var childInfo in directoryInfo.GetFileSystemInfos())
                {
                    DeleteFileSystemInfo(childInfo);
                }
            }

            fileSystemInfo.Attributes = FileAttributes.Normal;
            fileSystemInfo.Delete();
        }

        public bool Compile(Parkitect parkitect)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (!IsInstalled) throw new Exception("mod not installed");

            // Delete existing compiled file if compilation is forced.
            if (File.Exists(AssemblyPath))
                if (IsDevelopment)
                    File.Delete(AssemblyPath);
                else return true;

            using (var logFile = OpenLog())
            {
                try
                {
                    logFile.Log($"Compiling {Name}...");
                    logFile.Log($"Entry point: {NameSpace}.{ClassName}.{MethodName}.");

                    var assemblyFiles = new List<string>();
                    var sourceFiles = new List<string>();

                    var csProjPath = Project == null ? null : System.IO.Path.Combine(Path, BaseDir ?? "", Project);

                    List<string> unresolvedAssemblyReferences;
                    List<string> unresolvedSourceFiles;

                    if (csProjPath != null)
                    {
                        // Load source files and referenced assemblies from *.csproj file.
                        logFile.Log($"Compiling from `{Project}`.");

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

                        unresolvedAssemblyReferences = ReferencedAssemblies.ToList();
                        unresolvedSourceFiles = CodeFiles.ToList();
                    }

                    // Resolve the assembly references.
                    foreach (var name in unresolvedAssemblyReferences)
                    {
                        var resolved = ResolveAssembly(parkitect, name);
                        assemblyFiles.Add(resolved);

                        logFile.Log($"Resolved assembly reference `{name}` to `{resolved}`");
                    }

                    // Resolve the source file paths.
                    logFile.Log($"Source files: {String.Join(", ", unresolvedSourceFiles)}.");
                    sourceFiles.AddRange(
                        unresolvedSourceFiles.Select(file => System.IO.Path.Combine(Path, BaseDir ?? "", file)));

                    // Compile.
                    var csCodeProvider =
                        new CSharpCodeProvider(new Dictionary<string, string> {{"CompilerVersion", CompilerVersion}});
                    var parameters = new CompilerParameters(assemblyFiles.ToArray(), AssemblyPath);

                    var result = csCodeProvider.CompileAssemblyFromFile(parameters, sourceFiles.ToArray());

                    // Log errors.
                    foreach (var error in result.Errors.Cast<CompilerError>())
                        logFile.Log(
                            $"{error.ErrorNumber}: {error.Line}:{error.Column}: {error.ErrorText} in {error.FileName}",
                            LogLevel.Error);

                    return !result.Errors.HasErrors;
                }
                catch (Exception e)
                {
                    logFile.Log(e.Message, LogLevel.Error);
                    return false;
                }
            }
        }
    }
}