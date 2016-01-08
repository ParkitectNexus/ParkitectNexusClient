// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client
{
    public static class ModLoaderUtil
    {
        private static void InstallModLoaderFile(IParkitect parkitect, string fileName)
        {
            var targetDirectory = parkitect.Paths.NativeMods;

            Directory.CreateDirectory(targetDirectory);
            var targetPath = Path.Combine(targetDirectory, fileName);
            var sourcePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), fileName);

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

        public static void InstallModLoader(IParkitect parkitect)
        {
            try
            {
                InstallModLoaderFile(parkitect, "ParkitectNexus.Mod.ModLoader.dll");
            }
            catch (Exception e)
            {
                Logger.WriteLine("Failed to install mod loader.", LogLevel.Warn);
                Logger.WriteException(e, LogLevel.Warn);
            }
        }
    }
}