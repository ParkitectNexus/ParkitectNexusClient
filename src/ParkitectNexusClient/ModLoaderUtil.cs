// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client
{
    public static class ModLoaderUtil
    {
        public static void InstallModLoader(Parkitect parkitect)
        {
            try
            {
                const string modLoader = "ParkitectNexus.Mod.ModLoader.dll";
                var targetDirectory = parkitect.Paths["Mods"];

                Directory.CreateDirectory(targetDirectory);
                var targetPath = Path.Combine(targetDirectory, modLoader);
                var sourcePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), modLoader);

                if (!File.Exists(sourcePath))
                    return;

                if (File.Exists(targetPath))
                {
                    var md5 = MD5.Create();
                    byte[] sourceHash, targetHash;

                    using (var stream = File.OpenRead(sourcePath))
                        sourceHash = md5.ComputeHash(stream);
                    using (var stream = File.OpenRead(targetPath))
                        targetHash = md5.ComputeHash(stream);

                    if (sourceHash.SequenceEqual(targetHash))
                        return;
                }

                File.Copy(sourcePath, targetPath, true);
            }
            catch (Exception e)
            {
                Log.WriteLine("Failed to install mod loader.", LogLevel.Warn);
                Log.WriteException(e, LogLevel.Warn);
            }
        }
    }
}