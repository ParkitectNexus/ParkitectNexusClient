﻿// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Web.Client
{
    public interface INexusWebClient : IDisposable
    {
        HttpResponseHeaders ResponseHeaders { get; }

        Task<Stream> OpenReadTaskAsync(string url);
        void UploadString(string url, string data);
        Task<Stream> OpenRead(string url);
        void DownloadFile(string url, string path);
        Task<string> DownloadString(string url);
        void Authorize();
    }
}