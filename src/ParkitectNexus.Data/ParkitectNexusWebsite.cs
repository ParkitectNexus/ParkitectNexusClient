// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Win32;

namespace ParkitectNexus.Data
{
    /// <summary>
    ///     Represents the ParkitectNexus website.
    /// </summary>
    public class ParkitectNexusWebsite
    {
#if DEBUG
        private const string WebsiteUrl = "https://{0}staging.parkitectnexus.com/{1}";
#else
        private const string WebsiteUrl = "https://{0}parkitectnexus.com/{1}";
#endif

        /// <summary>
        ///     Launches the nexus.
        /// </summary>
        public void Launch()
        {
            Process.Start(ResolveUrl(null));
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
    }
}