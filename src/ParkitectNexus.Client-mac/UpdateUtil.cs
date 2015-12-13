// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;
using System.Linq;

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
        public static UpdateInfo CheckForUpdates(IParkitectNexusWebsite website)
        {
            try
            {
                using (var webClient = new ParkitectNexusWebClient())
                using (var stream = webClient.OpenRead(website.ResolveUrl("update-osx.json", "client")))
                using (var streamReader = new StreamReader(stream))
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    var serializer = new JsonSerializer();
                    var updateInfo = (UpdateInfo) serializer.Deserialize(jsonTextReader, typeof (UpdateInfo));

                    if (updateInfo != null)
                    {
                        var currentVersion = typeof (MainClass).Assembly.GetName().Version;
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
                var targetFolder = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "Downloads");

                if(!Directory.Exists(targetFolder))
                    targetFolder = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "Documents");

                if(!Directory.Exists(targetFolder))
                    targetFolder = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal));

                if(!Directory.Exists(targetFolder))
                    return false;

                var tempPath = Path.Combine(targetFolder, $"parkitectnexus-client-{update.Version}-{Random(6)}.dmg");

                using (var webClient = new ParkitectNexusWebClient())
                {
                    webClient.DownloadFile(update.DownloadUrl, tempPath);

                    Process.Start(new ProcessStartInfo(
                        "hdiutil",
                        "attach \"" + tempPath + "\"")
                    {UseShellExecute = false});
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string Random(int length)
        {
            var eligable = Enumerable.Range(0, 36).Select(n => n < 10 ? (char)(n + '0') : (char)('a' + n - 10)).ToArray();
            var random = new Random();
            return string.Concat(Enumerable.Range(0, length).Select(n => eligable[random.Next(eligable.Length)]));
        }
    }
}