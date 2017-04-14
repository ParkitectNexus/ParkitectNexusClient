// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.Client;
using OperatingSystem = ParkitectNexus.Data.Utilities.OperatingSystem;

namespace ParkitectNexus.Data.Updating
{
    public class UpdateManager : IUpdateManager
    {
        private readonly ILogger _log;
        private readonly INexusWebClientFactory _webClientFactory;
        private readonly IWebsite _website;

        public UpdateManager(IWebsite website, INexusWebClientFactory webClientFactory, ILogger log)
        {
            _website = website;
            _webClientFactory = webClientFactory;
            _log = log;
        }

        /// <summary>
        ///     Checks for available updates.
        /// </summary>
        /// <returns>Information about the available update.</returns>
        public async Task<UpdateInfo> CheckForUpdates<TEntryPoint>()
        {
            var currentVersion = typeof (TEntryPoint).Assembly.GetName().Version;

            try
            {
                _log.WriteLine(
                    $"Checking for client updates... Currently on version v{currentVersion}-{OperatingSystem.Detect()}.");

                using (var webClient = _webClientFactory.CreateWebClient(true))
                using (var stream = await webClient.OpenRead(GetUpdateVersionUrl()))
                using (var streamReader = new StreamReader(stream))
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    var serializer = new JsonSerializer();
                    var updateInfo = (UpdateInfo) serializer.Deserialize(jsonTextReader, typeof (UpdateInfo));

                    if (updateInfo != null)
                    {
                        var newestVersion = new Version(updateInfo.Version);

                        _log.WriteLine($"Server reported newest version is v{updateInfo.Version}.");
                        if (newestVersion > currentVersion)
                        {
                            _log.WriteLine($"Found newer version v{newestVersion}.");
                            return updateInfo;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                //TODO: hotfix to avoid the application from crashing
                try{
                    _log.WriteLine(GetUpdateVersionUrl());
                    _log.WriteLine("Failed to check for updates");
                    _log.WriteException(e);
                }
                catch(Exception ex)
                {
                    _log.WriteException(ex);

                }
            }

            return null;
        }

        /// <summary>
        ///     Installs the specified update.
        /// </summary>
        /// <param name="update">The update.</param>
        /// <returns>true on success; false otherwise.</returns>
        public bool InstallUpdate(UpdateInfo update)
        {
            try
            {
                switch (OperatingSystem.Detect())
                {
                    case SupportedOperatingSystem.Windows:
                    {
                        var tempPath = Path.Combine(Path.GetTempPath(), "pncsetup.msi");

                        using (var webClient = _webClientFactory.CreateWebClient(true))
                        {
                            webClient.DownloadFile(update.DownloadUrl, tempPath);
                            Process.Start(tempPath);
                        }
                        break;
                    }
                    case SupportedOperatingSystem.MacOSX:
                    {
                        var targetFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                            "Downloads");

                        if (!Directory.Exists(targetFolder))
                            targetFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                                "Documents");

                        if (!Directory.Exists(targetFolder))
                            targetFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal));

                        if (!Directory.Exists(targetFolder))
                            return false;

                        var tempPath = Path.Combine(targetFolder,
                            $"parkitectnexus-client-{update.Version}-{RandomString(6)}.dmg");

                        using (var webClient = _webClientFactory.CreateWebClient())
                        {
                            webClient.DownloadFile(update.DownloadUrl, tempPath);

                            Process.Start(new ProcessStartInfo(
                                "hdiutil",
                                "attach \"" + tempPath + "\"")
                            {UseShellExecute = false});
                        }
                        break;
                    }
                    case SupportedOperatingSystem.Linux:
                        throw new NotImplementedException();
                    default:
                        throw new Exception("unknown operating system");
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        protected virtual string GetUpdateVersionUrl()
        {
            switch (OperatingSystem.Detect())
            {
                case SupportedOperatingSystem.Windows:
                    return _website.ResolveUrl("update.json", "client");
                case SupportedOperatingSystem.MacOSX:
                    return _website.ResolveUrl("update-osx.json", "client");
                case SupportedOperatingSystem.Linux:
                    throw new NotImplementedException();
                default:
                    throw new Exception("unknown operating system");
            }
        }

        private static string RandomString(int length)
        {
            var eligable =
                Enumerable.Range(0, 36).Select(n => n < 10 ? (char) (n + '0') : (char) ('a' + n - 10)).ToArray();
            var random = new Random();
            return string.Concat(Enumerable.Range(0, length).Select(n => eligable[random.Next(eligable.Length)]));
        }
    }
}
