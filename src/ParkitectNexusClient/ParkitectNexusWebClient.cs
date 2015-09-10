// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Net;

namespace ParkitectNexusClient
{
    internal class ParkitectNexusWebClient : WebClient
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Net.WebClient" /> class.
        /// </summary>
        public ParkitectNexusWebClient()
        {
            // Add a version number header of requests.
            Headers.Add("X-ParkitectNexusInstaller-Version",
                typeof (ParkitectNexusWebClient).Assembly.GetName().Version.ToString());
        }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            var w = base.GetWebRequest(uri);
            w.Timeout = 10*60*1000;
            return w;
        }
    }
}