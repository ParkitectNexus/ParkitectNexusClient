// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Octokit;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web.Client;

namespace ParkitectNexus.Data.Web
{
    /// <summary>
    ///     Represents the online parkitect asset storage.
    /// </summary>
    public class ParkitectOnlineAssetRepository : IParkitectOnlineAssetRepository
    {
        private readonly IGitHubClient _gitClient;
        private readonly IParkitectNexusWebsite _parkitectNexusWebsite;
        private readonly ILogger _logger;
        private readonly IParkitectNexusWebFactory _nexusWebFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParkitectOnlineAssetRepository"/> class.
        /// </summary>
        /// <param name="gitClient">The git client.</param>
        /// <param name="parkitectNexusWebsite">The parkitect nexus website.</param>
        /// <param name="nexusWebFactory">The nexus web factory.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">gitClient, parkitectNexusWebsite, nexusWebFactory or logger is null.</exception>
        public ParkitectOnlineAssetRepository(IGitHubClient gitClient, IParkitectNexusWebsite parkitectNexusWebsite,
            IParkitectNexusWebFactory nexusWebFactory, ILogger logger)
        {
            if (gitClient == null) throw new ArgumentNullException(nameof(gitClient));
            if (parkitectNexusWebsite == null) throw new ArgumentNullException(nameof(parkitectNexusWebsite));
            if (nexusWebFactory == null) throw new ArgumentNullException(nameof(nexusWebFactory));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _parkitectNexusWebsite = parkitectNexusWebsite;
            _nexusWebFactory = nexusWebFactory;
            _gitClient = gitClient;
            _logger = logger;
        }

        /// <summary>
        ///     Resolves the download info for the specified url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Information about the download.</returns>
        public async Task<DownloadInfo> ResolveDownloadInfo(IParkitectNexusUrl url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            switch (url.AssetType)
            {
                case ParkitectAssetType.Blueprint:
                case ParkitectAssetType.Savegame:
                    return new DownloadInfo(_parkitectNexusWebsite.ResolveUrl($"download/{url.FileHash}"), null, null);
                case ParkitectAssetType.Mod:
                    var tag = await GetLatestModTag(url.FileHash, _gitClient);
                    if (tag == null)
                        throw new Exception("mod has not yet been released(tagged)");
                    return new DownloadInfo(tag.ZipballUrl, url.FileHash, tag.Name);
                default:
                    throw new Exception("unsupported mod type");
            }
        }

        /// <summary>
        ///     Downloads the file associated with the specified url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>An instance which performs the requested task.</returns>
        public async Task<IParkitectDownloadedAsset> DownloadFile(IParkitectNexusUrl url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            // Create a download url based on the file hash.
            var downloadInfo = await ResolveDownloadInfo(url);

            // Create a web client which will download the file.
            using (var webClient = _nexusWebFactory.CreateWebClient())
            {
                // Receive the content of the file.
                using (var stream = await webClient.OpenReadTaskAsync(downloadInfo.Url))
                {
                    // Read content information from the headers.
                    var contentDispositionHeader = webClient.ResponseHeaders.Get("Content-Disposition");
                    var contentLengthHeader = webClient.ResponseHeaders.Get("Content-Length");
                    var contentTypeHeader = webClient.ResponseHeaders.Get("Content-Type");

                    // Ensure the required content headers exist.
                    if (string.IsNullOrWhiteSpace(contentDispositionHeader) ||
                        string.IsNullOrWhiteSpace(contentTypeHeader))
                        throw new Exception("invalid headers");

                    // Parse the content length header to an integer.
                    var contentLength = 0;
                    if (contentLengthHeader != null && !int.TryParse(contentLengthHeader, out contentLength))
                        throw new Exception("invalid headers");

                    // Get asset information for the asset type specified in the url.
                    var assetInfo = url.AssetType.GetCustomAttribute<ParkitectAssetInfoAttribute>();

                    // Ensure the type of the received content matches the expected content type.
                    if (assetInfo == null || assetInfo.ContentType != contentTypeHeader.Split(';').FirstOrDefault())
                        throw new Exception("invalid response type");

                    // Extract the filename of the asset from the content disposition header.
                    var fileNameMatch = Regex.Match(contentDispositionHeader, @"attachment; filename=(""?)(.*)\1");

                    if (fileNameMatch == null || !fileNameMatch.Success)
                        throw new Exception("invalid headers");

                    var fileName = fileNameMatch.Groups[2].Value;

                    // Copy the contents of the downloaded stream to a memory stream.
                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);

                    // Verify we received all content.
                    if (contentLengthHeader != null && memoryStream.Length != contentLength)
                        throw new Exception("unexpected end of stream");

                    // Create an instance of ParkitectAsset with the received content and data.
                    return new ParkitectDownloadedAsset(fileName, downloadInfo, url.AssetType, memoryStream);
                }
            }
        }

        /// <summary>
        ///     Determines whether the specified input is valid file hash.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns>
        ///     true if valid; false otherwise.
        /// </returns>
        public static bool IsValidFileHash(string input, ParkitectAssetType assetType)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            switch (assetType)
            {
                case ParkitectAssetType.Blueprint:
                case ParkitectAssetType.Savegame:
                    return input.Length == 10 && input.All(c => (c >= 'a' && c <= 'f') || (c >= '0' && c <= '9'));
                case ParkitectAssetType.Mod:
                    var p = input.Split('/');
                    return p.Length == 2 && !string.IsNullOrWhiteSpace(p[0]) && !string.IsNullOrWhiteSpace(p[1]);
                default:
                    return false;
            }
        }

        protected virtual async Task<RepositoryTag> GetLatestModTag(string mod, IGitHubClient client)
        {
            if (mod == null) throw new ArgumentNullException(nameof(mod));
            var p = mod.Split('/');

            if (p.Length != 2 || string.IsNullOrWhiteSpace(p[0]) || string.IsNullOrWhiteSpace(p[1]))
                throw new ArgumentException(nameof(mod));

            _logger.WriteLine($"Getting latest mod tag for '{mod}'.");
            var release = (await client.Release.GetAll(p[0], p[1])).FirstOrDefault(r => !r.Prerelease);

            return release == null
                ? null
                : (await client.Repository.GetAllTags(p[0], p[1])).FirstOrDefault(t => t.Name == release.TagName);
        }
    }
}
