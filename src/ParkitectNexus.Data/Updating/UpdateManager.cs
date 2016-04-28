using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.Client;

namespace ParkitectNexus.Data.Updating
{
    public class UpdateManager : IUpdateManager
    {
        private readonly IWebsite _website;
        private readonly INexusWebClientFactory _webClientFactory;
        private readonly ILogger _log;

        public UpdateManager(IWebsite website, INexusWebClientFactory webClientFactory, ILogger log)
        {
            _website = website;
            _webClientFactory = webClientFactory;
            _log = log;
        }

        protected virtual string GetUpdateVersionUrl()
        {
            switch (Utilities.OperatingSystem.Detect())
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

        /// <summary>
        ///     Checks for available updates.
        /// </summary>
        /// <returns>Information about the available update.</returns>
        public UpdateInfo CheckForUpdates<TEntryPoint>()
        {
            var currentVersion = typeof (TEntryPoint).Assembly.GetName().Version;

            try
            {
                _log.WriteLine(
                    $"Checking for client updates... Currently on version v{currentVersion}-{Utilities.OperatingSystem.Detect()}.");

                using (var webClient = _webClientFactory.CreateWebClient(true))
                using (var stream = webClient.OpenRead(GetUpdateVersionUrl()))
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
        public bool InstallUpdate(UpdateInfo update)
        {
            try
            {
                switch (Utilities.OperatingSystem.Detect())
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

        private static string RandomString(int length)
        {
            var eligable = Enumerable.Range(0, 36).Select(n => n < 10 ? (char)(n + '0') : (char)('a' + n - 10)).ToArray();
            var random = new Random();
            return string.Concat(Enumerable.Range(0, length).Select(n => eligable[random.Next(eligable.Length)]));
        }
    }
}
