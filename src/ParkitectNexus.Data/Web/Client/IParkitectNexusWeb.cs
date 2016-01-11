// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Web.Client
{
    public interface IParkitectNexusWeb : IDisposable
    {
        WebHeaderCollection ResponseHeaders { get; }
        Task<Stream> OpenReadTaskAsync(string url);
        void UploadString(string url, string data);
        Stream OpenRead(string url);
        void DownloadFile(string url, string path);
    }
}