using System;
using System.Net;

namespace ParkitectNexusClient
{
    internal class ParkitectNexusWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            var w = base.GetWebRequest(uri);
            w.Timeout = 10 * 60 * 1000;
            return w;
        }
    }
}