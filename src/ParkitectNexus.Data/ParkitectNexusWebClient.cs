// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Net;
using System.Reflection;

namespace ParkitectNexus.Data
{
    public class ParkitectNexusWebClient : WebClient
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Net.WebClient" /> class.
        /// </summary>
        public ParkitectNexusWebClient()
        {
            // Add a version number header of requests.
            var version = Assembly.GetEntryAssembly().GetName().Version.ToString();
            Headers.Add("X-ParkitectNexusInstaller-Version", version);
            Headers.Add("user-agent", $"ParkitectNexus/{version}");
        }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            var request = base.GetWebRequest(uri);

            if (request == null) return null;

            if (request is HttpWebRequest)
            {
                var http = request as HttpWebRequest;
                http.KeepAlive = false;
                http.ServicePoint.Expect100Continue = false;
            }
            request.Timeout = 10*60*1000;
            return request;
        }
    }
}