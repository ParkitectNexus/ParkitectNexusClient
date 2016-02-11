// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Diagnostics;
using ParkitectNexus.Data.Web.API;
using ParkitectNexus.Data.Web.Client;

namespace ParkitectNexus.Data.Web
{
    /// <summary>
    ///     Represents the ParkitectNexus website.
    /// </summary>
    public class Website : IWebsite
    {
#if DEBUG
        private const string WebsiteUrl = "http://{0}staging.parkitectnexus.com/{1}";
#else
        private const string WebsiteUrl = "http://{0}parkitectnexus.com/{1}";
#endif

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public Website(INexusWebClientFactory webClientFactory)
        {
            API = new ParkitectNexusAPI(this, webClientFactory);
        }

        /// <summary>
        ///     Launches the nexus.
        /// </summary>
        public void Launch()
        {
            Launch(null, null);
        }

        /// <summary>
        ///     Launches the website at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Launch(string path)
        {
            Launch(path, null);
        }

        /// <summary>
        ///     Launches the website at the specified path and sub domain.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="subdomain">The sub domain.</param>
        public void Launch(string path, string subdomain)
        {
            Process.Start(ResolveUrl(path, subdomain));
        }

        /// <summary>
        ///     Resolves the URL to the specified path and subdomain.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="subdomain">The sub domain.</param>
        /// <returns>The URL.</returns>
        public string ResolveUrl(string path, string subdomain)
        {
            return string.Format(WebsiteUrl, string.IsNullOrEmpty(subdomain) ? string.Empty : subdomain + ".", path);
        }

        /// <summary>
        ///     Resolves the URL to the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The URL.</returns>
        public string ResolveUrl(string path)
        {
            return ResolveUrl(path, null);
        }

        /// <summary>
        ///     Gets the API.
        /// </summary>
        public IParkitectNexusAPI API { get; }
    }
}