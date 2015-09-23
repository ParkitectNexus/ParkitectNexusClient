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
            Headers.Add("X-ParkitectNexusInstaller-Version", Assembly.GetEntryAssembly().GetName().Version.ToString());
        }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            var w = base.GetWebRequest(uri);
            w.Timeout = 10*60*1000;
            return w;
        }
    }
}