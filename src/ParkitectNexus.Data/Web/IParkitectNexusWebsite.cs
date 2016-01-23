// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Data.Web.API;

namespace ParkitectNexus.Data.Web
{
    /// <summary>
    ///     Provides the functionality of the ParkitectNexus website.
    /// </summary>
    public interface IParkitectNexusWebsite
    {
        /// <summary>
        ///     Launches the nexus.
        /// </summary>
        void Launch();

        /// <summary>
        ///     Resolves the URL to the specified path and subdomain.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="subdomain">The sub domain.</param>
        /// <returns>The URL.</returns>
        string ResolveUrl(string path, string subdomain);

        /// <summary>
        ///     Resolves the URL to the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The URL.</returns>
        string ResolveUrl(string path);

        IParkitectNexusAPI API { get; }
    }
}
