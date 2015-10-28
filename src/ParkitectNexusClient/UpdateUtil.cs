// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using ParkitectNexus.Client.Properties;
using ParkitectNexus.Data;

namespace ParkitectNexus.Client
{
    /// <summary>
    ///     Contains utilities for updating.
    /// </summary>
    internal static class UpdateUtil
    {
        /// <summary>
        ///     Migrates settings from previous versions.
        /// </summary>
        public static void MigrateSettings()
        {
            if (Settings.Default.IsFirstRun)
            {
                Settings.Default.Upgrade();
                Settings.Default.IsFirstRun = false;
                Settings.Default.Save();
            }
        }

        /// <summary>
        ///     Checks for available updates.
        /// </summary>
        /// <returns>Information about the available update.</returns>
        public static UpdateInfo CheckForUpdates(ParkitectNexusWebsite website)
        {
            try
            {
                using (var webClient = new ParkitectNexusWebClient())
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
        public static bool InstallUpdate(UpdateInfo update)
        {
            try
            {
                var tempPath = Path.Combine(Path.GetTempPath(), "pncsetup.msi");

                using (var webClient = new WebClient())
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
    }
}