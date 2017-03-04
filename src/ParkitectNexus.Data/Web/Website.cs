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

using System.Diagnostics;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web.API;

namespace ParkitectNexus.Data.Web
{
    /// <summary>
    ///     Represents the ParkitectNexus website.
    /// </summary>
    public class Website : IWebsite
    {
        private readonly ILogger _log;
//#if DEBUG
//        private const string WebsiteUrl = "http://{0}dev.parkitectnexus.com/{1}";
//#else
        private const string WebsiteUrl = "http://{0}parkitectnexus.com/{1}";
//#endif

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public Website(ILogger log)
        {
            _log = log;
            API = ObjectFactory.With<IWebsite>(this).GetInstance<IParkitectNexusAPI>();
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
#if DEBUG
            if (subdomain == "client")
                subdomain = null;
#endif

            var url = string.Format(WebsiteUrl, string.IsNullOrEmpty(subdomain) ? string.Empty : subdomain + ".", path);

            _log.WriteLine($"Resolved URL: {url}");
            return url;
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
