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
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Octokit;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web.API;
using ParkitectNexus.Data.Web.Client;

namespace ParkitectNexus.Data.Assets
{
    /// <summary>
    ///     Represents the online parkitect asset storage.
    /// </summary>
    public class RemoteAssetRepository : IRemoteAssetRepository
    {
        private readonly IGitHubClient _gitHubClient;
        private readonly ILogger _logger;
        private readonly INexusWebClientFactory _webClientFactory;

        public RemoteAssetRepository(ILogger logger,
            INexusWebClientFactory webClientFactory, IGitHubClient gitHubClient)
        {
            _logger = logger;
            _webClientFactory = webClientFactory;
            _gitHubClient = gitHubClient;
        }

        /// <summary>
        ///     Downloads the asset.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception">the specified asset id is invalid</exception>
        public async Task<IDownloadedAsset> DownloadAsset(ApiAsset asset)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));

            var downloadInfo = await ResolveDownloadInfo(asset);

            using (var webClient = _webClientFactory.CreateWebClient(asset.Type != AssetType.Mod))
            {
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
                    var assetInfo = asset.Type.GetCustomAttribute<AssetInfoAttribute>();

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
                    return new DownloadedAsset(fileName, asset, downloadInfo, memoryStream);
                }
            }
        }

        /// <summary>
        ///     Gets the latest mod tag.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <returns>The latest tag.</returns>
        public async Task<string> GetLatestModTag(IModAsset asset)
        {
            if (asset == null)
                throw new ArgumentNullException(nameof(asset));

            var repositoryTag = await GetLatestModTag(asset.Repository);
            return repositoryTag.Name;
        }

        private async Task<DownloadInfo> ResolveDownloadInfo(ApiAsset asset)
        {
            if (asset == null) throw new ArgumentNullException(nameof(asset));

            switch (asset.Type)
            {
                case AssetType.Blueprint:
                case AssetType.Savegame:
                    return new DownloadInfo(asset.DownloadUrl, null, null);
                case AssetType.Mod:
                    var repoUrl = ((await asset.GetResource()) as ApiModResource).Source;
                    var repoUrlParts = repoUrl.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    var repo = $"{repoUrlParts[repoUrlParts.Length - 2]}/{repoUrlParts[repoUrlParts.Length - 1]}";

                    var tag = await GetLatestModTag(repo);
                    if (tag == null)
                        throw new Exception("mod has not yet been released(tagged)");
                    return new DownloadInfo(tag.ZipballUrl, repo, tag.Name);
                default:
                    throw new Exception("unsupported mod type");
            }
        }

        private async Task<RepositoryTag> GetLatestModTag(string repository)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            var p = repository.Split('/');

            if (p.Length != 2 || string.IsNullOrWhiteSpace(p[0]) || string.IsNullOrWhiteSpace(p[1]))
                throw new ArgumentException(nameof(repository));

            _logger.WriteLine($"Getting latest mod tag for '{repository}'.");

            var release = (await _gitHubClient.Repository.Release.GetAll(p[0], p[1])).FirstOrDefault(r => !r.Prerelease);

            return release == null
                ? null
                : (await _gitHubClient.Repository.GetAllTags(p[0], p[1])).FirstOrDefault(t => t.Name == release.TagName);
        }
    }
}