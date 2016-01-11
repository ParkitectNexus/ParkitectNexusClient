// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.Client;

namespace ParkitectNexus.Client
{
    /// <summary>
    ///     Contains utilities for updating.
    /// </summary>
    internal static class UpdateUtil
    {
        /// <summary>
        ///     Checks for available updates.
        /// </summary>
        /// <returns>Information about the available update.</returns>
        public static UpdateInfo CheckForUpdates(IParkitectNexusWebsite website, IParkitectNexusWebFactory webFactory)
        {
            try
            {
                using (var webClient = webFactory.NexusClient())
                using (var stream = webClient.OpenRead(website.ResolveUrl("update.json", "client")))
                using (var streamReader = new StreamReader(stream))
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    var serializer = new JsonSerializer();
                    var updateInfo = (UpdateInfo) serializer.Deserialize(jsonTextReader, typeof (UpdateInfo));

                    if (updateInfo != null)
                    {
                        var currentVersion = typeof (Program).Assembly.GetName().Version;
                        var newestVersion = new Version(updateInfo.Version);

                        currentVersion = new Version(currentVersion.Major, currentVersion.Minor, currentVersion.Build);
                        newestVersion = new Version(newestVersion.Major, newestVersion.Minor, newestVersion.Build);

                        if (newestVersion > currentVersion)
                            return updateInfo;
                    }
                }
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        ///     Installs the specified update.
        /// </summary>
        /// <param name="update">The update.</param>
        /// <returns>true on success; false otherwise.</returns>
        public static bool InstallUpdate(UpdateInfo update, IParkitectNexusWebFactory webFactory)
        {
            try
            {
                var tempPath = Path.Combine(Path.GetTempPath(), "pncsetup.msi");

                using (var webClient = webFactory.NexusClient())
                {
                    webClient.DownloadFile(update.DownloadUrl, tempPath);
                    Process.Start(tempPath);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void MigrateModsPreAlpha4ToPreAlpha5(IParkitect parkitect)
        {
            var oldPath = parkitect.Paths.GetPathInGameFolder("Mods");

            if (!Directory.Exists(oldPath))
                return;

            foreach (var directory in Directory.GetDirectories(oldPath))
            {
                var target = Path.Combine(parkitect.Paths.Mods, Path.GetFileName(directory));

                if (!File.Exists(Path.Combine(directory, "mod.json")) || Directory.Exists(target))
                    continue;

                Directory.Move(directory, target);
            }
        }

        public static void MoveFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);

                if (!File.Exists(dest))
                    File.Copy(file, dest);
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);

                if (!Directory.Exists(dest))
                    MoveFolder(folder, dest);
            }
        }

        private static void MigrateModsPreAlpha5ToPreAlpha6(IParkitect parkitect)
        {
            try
            {
                if (File.Exists(parkitect.Paths.GetPathInGameFolder("Mods/ParkitectNexus.Mod.ModLoader.dll")))
                    File.Delete(parkitect.Paths.GetPathInGameFolder("Mods/ParkitectNexus.Mod.ModLoader.dll"));

                if (!File.Exists(Path.Combine(parkitect.Paths.GetPathInGameFolder("Mods"), "migrated.1.3.pn")))
                {
                    MoveFolder(parkitect.Paths.GetPathInGameFolder("Mods"), parkitect.Paths.NativeMods);

                    File.Create(Path.Combine(parkitect.Paths.GetPathInGameFolder("Mods"), "migrated.1.3.pn"));
                }

                if (!File.Exists(Path.Combine(parkitect.Paths.GetPathInGameFolder("pnmods"), "migrated.1.3.pn")))
                {
                    MoveFolder(parkitect.Paths.GetPathInGameFolder("pnmods"), parkitect.Paths.Mods);
                    File.Create(Path.Combine(parkitect.Paths.GetPathInGameFolder("pnmods"), "migrated.1.3.pn"));
                }
            }
            catch (Exception)
            {
                // ignore
            }
        }

        public static void MigrateMods(IParkitect parkitect)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (!parkitect.IsInstalled)
                return;

            // MigrateModsPreAlpha4ToPreAlpha5(parkitect);
            MigrateModsPreAlpha5ToPreAlpha6(parkitect);
        }
    }
}