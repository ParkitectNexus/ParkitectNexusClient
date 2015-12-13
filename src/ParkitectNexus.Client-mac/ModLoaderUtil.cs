using System;
using ParkitectNexus.Data.Game;
using System.IO;
using MonoMac.Foundation;
using System.Security.Cryptography;
using ParkitectNexus.Data.Utilities;
using System.Linq;

namespace ParkitectNexus.Client
{
    public static class ModLoaderUtil
    {
        private static void InstallModLoaderFile(IParkitect parkitect, string fileName)
        {
            var targetDirectory = parkitect.Paths.NativeMods;

            Directory.CreateDirectory(targetDirectory);
            var targetPath = Path.Combine(targetDirectory, fileName);
            var sourcePath = Path.Combine(NSBundle.MainBundle.PathForResource("ParkitectNexus.Mod.ModLoader", "dll"));

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
                Log.WriteLine("Failed to install mod loader.", LogLevel.Warn);
                Log.WriteException(e, LogLevel.Warn);
            }
        }
    }
}

