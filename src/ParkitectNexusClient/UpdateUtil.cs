// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System.Diagnostics;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using ParkitectNexus.Data;
using ParkitectNexus.Client.Properties;

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

        public static UpdateInfo FindUpdate()
        {
            try
            {
                using (var webClient = new ParkitectNexusWebClient())
                using (var stream = webClient.OpenRead("https://client.parkitectnexus.com/update.json"))
                using (var streamReader = new StreamReader(stream))
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    var serializer = new JsonSerializer();
                    var updateInfo = (UpdateInfo) serializer.Deserialize(jsonTextReader, typeof (UpdateInfo));

                    return updateInfo.Version == typeof (Program).Assembly.GetName().Version.ToString()
                        ? null
                        : updateInfo;
                }
            }
            catch
            {
                return null;
            }
        }

        public static bool InstallUpdate(UpdateInfo updateInfo)
        {
            try
            {
                var tempPath = Path.Combine(Path.GetTempPath(), "pncsetup.msi");

                using (var webClient = new WebClient())
                {
                    webClient.DownloadFile(updateInfo.DownloadUrl, tempPath);
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

    [JsonObject]
    internal class UpdateInfo
    {
        public string Version { get; set; }

        [JsonProperty("download_url")]
        public string DownloadUrl { get; set; }
    }
}