// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using System.Reflection;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client.Windows
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
                using (var stream = File.OpenRead(sourcePath))
                using (var stream2 = File.OpenRead(targetPath))
                    if (stream.CreateMD5Checksum() == stream2.CreateMD5Checksum())
                        return;
            }

            File.Copy(sourcePath, targetPath, true);
        }

        public static void InstallModLoader(IParkitect parkitect, ILogger logger)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            try
            {
                InstallModLoaderFile(parkitect, "ParkitectNexus.Mod.ModLoader.dll");
            }
            catch (Exception e)
            {
                logger.WriteLine("Failed to install mod loader.", LogLevel.Warn);
                logger.WriteException(e, LogLevel.Warn);
            }
        }
    }
}
