// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Data.Web
{
    /// <summary>
    ///     Contains information about a downloadable file.
    /// </summary>
    public struct DownloadInfo
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DownloadInfo" /> struct.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="tag">The tag.</param>
        /// <exception cref="ArgumentNullException">Thrown if url is null.</exception>
        public DownloadInfo(string url, string repository, string tag)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            Url = url;
            Repository = repository;
            Tag = tag;
        }

        /// <summary>
        ///     Gets the URL.
        /// </summary>
        public string Url { get; }

        /// <summary>
        ///     Gets the GitHub repository.
        /// </summary>
        public string Repository { get; }

        /// <summary>
        ///     Gets the GitHub tag.
        /// </summary>
        public string Tag { get; }
    }
}