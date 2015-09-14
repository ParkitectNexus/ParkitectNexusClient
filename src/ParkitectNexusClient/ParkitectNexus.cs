// ParkitectNexusClient
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

namespace ParkitectNexusClient
{
    /// <summary>
    ///     Represents the ParkitectNexus server.
    /// </summary>
    internal class ParkitectNexus
    {
        private const string DownloadUrl = "https://parkitectnexus.com/download/{0}";

        /// <summary>
        ///     Launches the nexus.
        /// </summary>
        public void Launch()
        {
            Process.Start("https://parkitectnexus.com");
        }

        /// <summary>
        ///     Installs the parkitectnexus:// protocol.
        /// </summary>
        public void InstallProtocol()
        {
            try
            {
                var appPath = Assembly.GetEntryAssembly().Location;

                var parkitectNexus = Registry.CurrentUser?.CreateSubKey(@"Software\Classes\parkitectnexus");
                parkitectNexus?.SetValue("", "ParkitectNexus Client");
                parkitectNexus?.SetValue("URL Protocol", "");
                parkitectNexus?.CreateSubKey(@"DefaultIcon")?.SetValue("", $"{appPath},0");
                parkitectNexus?.CreateSubKey(@"shell\open\command")?.SetValue("", $"\"{appPath}\" --download \"%1\"");
            }
            catch (Exception)
            {
                // todo: Log the error or something. The app is useless without the url protocol.
            }
        }

        /// <summary>
        ///     Determines whether the specified input is valid file hash.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>true if valid; false otherwise.</returns>
        public static bool IsValidFileHash(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            return input.Length == 10 && input.All(c => (c >= 'a' && c <= 'f') || (c >= '0' && c <= '9'));
        }

        /// <summary>
        ///     Downloads the file associated with the specified url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>An instance which performs the requested task.</returns>
        public async Task<ParkitectAsset> DownloadFile(ParkitectNexusUrl url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            // Create a download url based on the file hash.
            var downloadUrl = string.Format(DownloadUrl, url.FileHash);

            // Create a web client which will download the file.
            using (var webClient = new ParkitectNexusWebClient())
            {
                // Receive the content of the file.
                using (var stream = await webClient.OpenReadTaskAsync(downloadUrl))
                {
                    // Read content information from the headers.
                    var contentDispositionHeader = webClient.ResponseHeaders.Get("Content-Disposition");
                    var contentLengthHeader = webClient.ResponseHeaders.Get("Content-Length");
                    var contentTypeHeader = webClient.ResponseHeaders.Get("Content-Type");

                    // Ensure the required content headers exist.
                    if (string.IsNullOrWhiteSpace(contentDispositionHeader) ||
                        string.IsNullOrWhiteSpace(contentLengthHeader) ||
                        string.IsNullOrWhiteSpace(contentTypeHeader))
                        throw new Exception("invalid headers");

                    // Parse the content length header to an integer.
                    int contentLength;
                    if (!int.TryParse(contentLengthHeader, out contentLength))
                        throw new Exception("invalid headers");

                    // Get asset information for the asset type specified in the url.
                    var assetInfo = url.AssetType.GetCustomAttribute<ParkitectAssetInfoAttribute>();

                    // Ensure the type of the received content matches the expected content type.
                    if (assetInfo == null || assetInfo.ContentType != contentTypeHeader.Split(';').FirstOrDefault())
                        throw new Exception("invalid response type");

                    // Extract the filename of the asset from the content disposition header.
                    var fileNameMatch = Regex.Match(contentDispositionHeader, @"attachment; filename=""(.*)""");

                    if (fileNameMatch == null || !fileNameMatch.Success)
                        throw new Exception("invalid headers");

                    var fileName = fileNameMatch.Groups[1].Value;

                    // Copy the contents of the downloaded stream to a memory stream.
                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);

                    // Verify we received all content.
                    if (memoryStream.Length != contentLength)
                        throw new Exception("unexpected end of stream");

                    // Create an instance of ParkitectAsset with the received content and data.
                    return new ParkitectAsset(fileName, url.AssetType, memoryStream);
                }
            }
        }
    }
}