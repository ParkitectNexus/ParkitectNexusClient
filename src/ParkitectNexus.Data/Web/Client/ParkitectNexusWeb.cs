// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Web.Client
{
    internal class ParkitectNexusWeb : IParkitectNexusWeb
    {
        private readonly ParkitectNexusWebClient _webClient;

        public ParkitectNexusWeb(IOperatingSystem operatingSystem)
        {
            _webClient = new ParkitectNexusWebClient(operatingSystem);
        }

        public WebHeaderCollection ResponseHeaders
        {
            get { return _webClient.ResponseHeaders; }
        }

        public void Dispose()
        {
            _webClient.Dispose();
        }

        public void DownloadFile(string url, string path)
        {
            _webClient.DownloadFile(url, path);
        }

        public Stream OpenRead(string url)
        {
            return _webClient.OpenRead(url);
        }

        public Task<Stream> OpenReadTaskAsync(string url)
        {
            return _webClient.OpenReadTaskAsync(url);
        }

        public void UploadString(string url, string data)
        {
            _webClient.UploadString(url, data);
        }
    }
}