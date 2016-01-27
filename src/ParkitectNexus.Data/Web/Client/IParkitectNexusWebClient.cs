// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Web.Client
{
    public interface IParkitectNexusWebClient : IDisposable
    {
        WebHeaderCollection ResponseHeaders { get; }
        Task<Stream> OpenReadTaskAsync(string url);
        string UploadString(string url, string data);
        Stream OpenRead(string url);
        void DownloadFile(string url, string path);
        void Authorize();
    }
}
