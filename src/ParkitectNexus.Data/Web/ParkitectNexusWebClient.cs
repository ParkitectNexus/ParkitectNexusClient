// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Net;
using System.Reflection;

namespace ParkitectNexus.Data.Web
{
    public class ParkitectNexusWebClient : WebClient
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Net.WebClient" /> class.
        /// </summary>
        public ParkitectNexusWebClient()
        {
            // Add a version number header of requests.
            var version = $"{Assembly.GetEntryAssembly().GetName().Version}-{OperatingSystems.GetOperatingSystem()}";

            Headers.Add("X-ParkitectNexusInstaller-Version", version);
            Headers.Add("user-agent", $"ParkitectNexus/{version}");
        }

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