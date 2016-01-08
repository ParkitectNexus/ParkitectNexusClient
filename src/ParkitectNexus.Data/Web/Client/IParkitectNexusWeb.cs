using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Web.Client
{
    public interface IParkitectNexusWeb : IDisposable
    {
        Task<Stream> OpenReadTaskAsync(string url);
        WebHeaderCollection ResponseHeaders { get; }
        void UploadString(string url,string data);
        Stream OpenRead(string url);
        void DownloadFile(string url, string path);
    }
}
