// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Net;
using System.Net.Cache;
using System.Reflection;

namespace ParkitectNexus.Data.Web.Client
{
    internal class ParkitectNexusWebClient : WebClient
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Net.WebClient" /> class.
        /// </summary>
        public ParkitectNexusWebClient(IOperatingSystem operatingSystem)
        {
            // Workaround: disable certificate cache on MacOSX.
            if (operatingSystem.GetOperatingSystem() == SupportedOperatingSystem.MacOSX)
            {
                CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            }

            if (operatingSystem.GetOperatingSystem() == SupportedOperatingSystem.MacOSX)
            {
                ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
            }


            // Add a version number header of requests.
            var version = $"{Assembly.GetEntryAssembly().GetName().Version}-{operatingSystem.GetOperatingSystem()}";

            Headers.Add("X-ParkitectNexusInstaller-Version", version);
            Headers.Add("user-agent", $"ParkitectNexus/{version}");
        }

        /*  static ParkitectNexusWebClient()
        {
            // Workaround: bypass certificate checks on MacOSX.
            if(operatingSystem.GetOperatingSystem() == SupportedOperatingSystem.MacOSX)
            {
                ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
            }
        }*/

        /// <summary>
        ///     Returns a <see cref="T:System.Net.WebRequest" /> object for the specified resource.
        /// </summary>
        /// <returns>
        ///     A new <see cref="T:System.Net.WebRequest" /> object for the specified resource.
        /// </returns>
        /// <param name="uri">A <see cref="T:System.Uri" /> that identifies the resource to request.</param>
        protected override WebRequest GetWebRequest(Uri uri)
        {
            var request = base.GetWebRequest(uri);

            if (request == null) return null;

            // Some jumble for downloading from github.
            if (request is HttpWebRequest)
            {
                var http = request as HttpWebRequest;
                http.KeepAlive = false;
                http.ServicePoint.Expect100Continue = false;
            }

            // Set a 10 minute timeout. Should allow the slowest of connections to download anything.
            request.Timeout = 10*60*1000;
            return request;
        }
    }
}