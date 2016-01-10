using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Web.Client
{
    class ParkitectNexusWeb : IParkitectNexusWeb
    {
        private ParkitectNexusWebClient _webClient;

        public ParkitectNexusWeb(IOperatingSystem operatingSystem)
        {
            _webClient = new ParkitectNexusWebClient(operatingSystem);
        }

        public WebHeaderCollection ResponseHeaders
        {
            get
            {
                return _webClient.ResponseHeaders;
            }
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

        public void UploadString(string url,string data)
        {
            _webClient.UploadString(url, data);
        }
    }
}
