// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Data.Assets
{
    public struct DownloadInfo
    {
        public DownloadInfo(string url, string repository, string tag)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            Url = url;
            Repository = repository;
            Tag = tag;
        }

        public string Url { get; }

        public string Repository { get; }

        public string Tag { get; }
    }
}
