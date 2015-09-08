// ParkitectNexusInstaller
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace ParkitectNexusInstaller
{
    internal class ParkitectNexus
    {
        private const string DownloadUrl = "https://parkitectnexus.com/download/{0}";

        public void Launch()
        {
            Process.Start("https://parkitectnexus.com");
        }

        public void InstallProtocol()
        {
            try
            {
                var appPath = Assembly.GetEntryAssembly().Location;

                var parkitectNexus = Registry.CurrentUser?.CreateSubKey(@"Software\Classes\parkitectnexus");
                parkitectNexus?.SetValue("", "Parkitect Nexus Blueprint Installer");
                parkitectNexus?.SetValue("URL Protocol", "");
                parkitectNexus?.CreateSubKey(@"DefaultIcon")?.SetValue("", $"{appPath},0");
                parkitectNexus?.CreateSubKey(@"shell\open\command")?.SetValue("", $"\"{appPath}\" --download \"%1\"");
            }
            catch (Exception)
            {
                // todo: Log the error or something. The app is useless without the url protocol.
            }
        }

        public static bool IsValidFileHash(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            return input.Length == 10 && input.All(c => (c >= 'a' && c <= 'f') || (c >= '0' && c <= '9'));
        }

        public async Task<ParkitectAsset> DownloadFile(ParkitectNexusUrl url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            var downloadUrl = string.Format(DownloadUrl, url.FileHash);
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("X-ParkitectNexus-Installer-Version",
                    Assembly.GetExecutingAssembly().GetName().Version.ToString());

                using (var stream = await webClient.OpenReadTaskAsync(downloadUrl))
                {
                    var contentDispositionHeader = webClient.ResponseHeaders.Get("Content-Disposition");
                    var contentLengthHeader = webClient.ResponseHeaders.Get("Content-Length");
                    var contentTypeHeader = webClient.ResponseHeaders.Get("Content-Type");

                    if (string.IsNullOrWhiteSpace(contentDispositionHeader) ||
                        string.IsNullOrWhiteSpace(contentLengthHeader) ||
                        string.IsNullOrWhiteSpace(contentTypeHeader))
                        throw new Exception("invalid headers");

                    int contentLength;
                    if (!int.TryParse(contentLengthHeader, out contentLength))
                        throw new Exception("invalid headers");

                    var assetInfo = url.AssetType.GetCustomAttribute<ParkitectAssetInfoAttribute>();
                    if (assetInfo == null || assetInfo.ContentType != contentTypeHeader.Split(';').FirstOrDefault())
                        throw new Exception("invalid response type");

                    var fileNameMatch = Regex.Match(contentDispositionHeader, @"attachment; filename=""(.*)""");

                    if (fileNameMatch == null || !fileNameMatch.Success)
                        throw new Exception("invalid headers");

                    var fileName = fileNameMatch.Groups[1].Value;

                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);

                    if (memoryStream.Length != contentLength)
                        throw new Exception("unexpected end of stream");

                    return new ParkitectAsset(fileName, url.AssetType, memoryStream);
                }
            }
        }
    }
}