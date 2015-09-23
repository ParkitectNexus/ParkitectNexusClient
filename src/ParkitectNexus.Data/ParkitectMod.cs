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
        public string Repository { get; set; }


        public bool Compile(Parkitect parkitect)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if(Path == null) throw new Exception("mod not installed");
            
            // Delete existing compiled file if compilation is forced.
            if (File.Exists(AssemblyPath))
                if (ForceCompile)
                    File.Delete(AssemblyPath);
                else return true;
            
            // Compile files with default references.
            var csCodeProvider = new CSharpCodeProvider(new Dictionary<string, string>() {{"CompilerVersion", "v4.0"}});
            var parameters =
                new CompilerParameters(
                    new[]
                    {
                        "UnityEngine.dll", "UnityEngine.UI.dll", "Assembly-CSharp.dll"
                    }.Select(n => System.IO.Path.Combine(parkitect.ManagedDataPath, n))
                        .ToArray()
                        .Concat(new[]
                        {
                            "mscorlib.dll", "System.dll", "System.Core.dll", "System.Data.dll"
                        })
                        .ToArray(),
                    AssemblyPath);

            var csFiles = new List<string>();

            var csProjPath = Project == null ? null : System.IO.Path.Combine(Path, Project);

            // Load files from csproj 
            if (csProjPath != null)
            {
                var document = new XmlDocument();
                document.Load(csProjPath);

                var manager = new XmlNamespaceManager(document.NameTable);
                manager.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

                csFiles.AddRange(document.SelectNodes("//x:Compile", manager)
                    .Cast<XmlNode>()
                    .Select(node => node.Attributes["Include"])
                    .Select(file => System.IO.Path.Combine(Path, file.Value)));
            }
            else
            {
                // Load from listed files
                if (CodeFiles != null)
                    csFiles.AddRange(CodeFiles.Select(n => System.IO.Path.Combine(Path, n)));
            }

            var results = csCodeProvider.CompileAssemblyFromFile(parameters, csFiles.ToArray());

            // results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
            // todo log errors and warnings to log file in mod folder

            return !results.Errors.HasErrors;
        }
    }
}