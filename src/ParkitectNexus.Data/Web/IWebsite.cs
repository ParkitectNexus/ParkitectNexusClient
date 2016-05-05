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

using ParkitectNexus.Data.Web.API;

namespace ParkitectNexus.Data.Web
{
    /// <summary>
    ///     Provides the functionality of the ParkitectNexus website.
    /// </summary>
    public interface IWebsite
    {
        IParkitectNexusAPI API { get; }

        /// <summary>
        ///     Launches the website.
        /// </summary>
        void Launch();

        /// <summary>
        ///     Launches the website at the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        void Launch(string path);

        /// <summary>
        ///     Launches the website at the specified path and sub domain.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="subdomain">The sub domain.</param>
        void Launch(string path, string subdomain);

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
    }
}