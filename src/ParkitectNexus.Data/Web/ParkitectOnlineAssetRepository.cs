// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Linq;
using System.Threading.Tasks;
using Octokit;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Web
{
    /// <summary>
    ///     Represents the online parkitect asset storage.
    /// </summary>
    public class ParkitectOnlineAssetRepository : IParkitectOnlineAssetRepository
    {
        private readonly ILogger _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ParkitectOnlineAssetRepository" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">gitClient, parkitectNexusWebsite, nexusWebFactory or logger is null.</exception>
        public ParkitectOnlineAssetRepository(ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
        }

        /// <summary>
        ///     Downloads the file associated with the specified url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>An instance which performs the requested task.</returns>
        public async Task<IParkitectDownloadedAsset> DownloadFile(IParkitectNexusUrl url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            throw new NotImplementedException();
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
