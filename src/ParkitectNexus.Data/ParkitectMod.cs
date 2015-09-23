// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            "System.Xml.Linq", "Microsoft.CSharp"
        };

        [JsonProperty]
        public string AssemblyFileName { get; set; }

        public string AssemblyPath => !IsInstalled ? null : System.IO.Path.Combine(Path, AssemblyFileName);

        [JsonProperty]
        public string ClassName { get; set; } = "Main";

        [JsonProperty]
        public IList<string> CodeFiles { get; set; } = new List<string>();

        [JsonProperty]
        public bool EnableLogging { get; set; } = true;

        [JsonProperty]
        public bool ForceCompile { get; set; }

        public bool IsInstalled
            => !string.IsNullOrWhiteSpace(Path) && File.Exists(System.IO.Path.Combine(Path, "mod.json"));

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

            var modPath = System.IO.Path.Combine(Path, dllName);
            if (File.Exists(System.IO.Path.Combine(modPath)))
                return modPath;

            throw new Exception($"Failed to resolve referenced assembly '{assemblyName}'");
        }
        
        public bool Compile(Parkitect parkitect)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (Path == null) throw new Exception("mod not installed");

            // Delete existing compiled file if compilation is forced.
            if (File.Exists(AssemblyPath))
                if (ForceCompile)
                    File.Delete(AssemblyPath);
                else return true;

            var assemblyFiles = new List<string>();
            var sourceFiles = new List<string>();

            var csProjPath = Project == null ? null : System.IO.Path.Combine(Path, Project);
            if (csProjPath != null)
            {
                // Load source files and referenced assemblies from *.csproj file.
                var document = new XmlDocument();
                document.Load(csProjPath);

                var manager = new XmlNamespaceManager(document.NameTable);
                manager.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

                assemblyFiles.AddRange(document.SelectNodes("//x:Reference", manager)
                    .Cast<XmlNode>()
                    .Select(node => node.Attributes["Include"])
                    .Select(name => name.Value.Split(',').FirstOrDefault())
                    .Select(name => ResolveAssembly(parkitect, name)));

                sourceFiles.AddRange(document.SelectNodes("//x:Compile", manager)
                    .Cast<XmlNode>()
                    .Select(node => node.Attributes["Include"])
                    .Select(file => System.IO.Path.Combine(Path, file.Value)));
            }
            else
            {
                // Load source files and referenced assemblies from mod.json file.
                assemblyFiles.AddRange(ReferencedAssemblies.Select(name => ResolveAssembly(parkitect, name)));
                sourceFiles.AddRange(CodeFiles.Select(file => System.IO.Path.Combine(Path, file)));
            }

            // Compile files with default references.
            var csCodeProvider = new CSharpCodeProvider(new Dictionary<string, string>() {{"CompilerVersion", "v4.0"}});
            var parameters = new CompilerParameters(assemblyFiles.ToArray(), AssemblyPath);

            var results = csCodeProvider.CompileAssemblyFromFile(parameters, sourceFiles.ToArray());

            // results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
            // todo log errors and warnings to log file in mod folder

            return !results.Errors.HasErrors;
        }
    }
}