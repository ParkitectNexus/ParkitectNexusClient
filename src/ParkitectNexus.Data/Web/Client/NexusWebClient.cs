// ParkitectNexusClient
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
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using ParkitectNexus.Data.Authentication;
using OperatingSystem = ParkitectNexus.Data.Utilities.OperatingSystem;

namespace ParkitectNexus.Data.Web.Client
{
    internal class NexusWebClient : HttpClient, INexusWebClient
    {
        private readonly IAuthManager _authManager;


        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Net.WebClient" /> class.
        /// </summary>
        public NexusWebClient(IAuthManager authManager)
        {
            _authManager = authManager;

            var os = OperatingSystem.Detect();

            // Workaround: disable certificate cache on MacOSX.
            if (os == SupportedOperatingSystem.MacOSX)
            {
                ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
            }

            // Add a version number header of requests.
            var version = $"{Assembly.GetEntryAssembly().GetName().Version}-{os}";
            
            DefaultRequestHeaders.Add("X-ParkitectNexusInstaller-Version", version);
            DefaultRequestHeaders.Add("user-agent", $"ParkitectNexus/{version}");
        }

        #region Implementation of INexusWebClient

        public void Authorize()
        {
            if (_authManager.IsAuthenticated)
                DefaultRequestHeaders.Add("Authorization", _authManager.Key);
        }

        #endregion
        
	    public async Task<Stream> OpenReadTaskAsync(string url)
        {
            using (HttpResponseMessage response = await GetAsync(url))
            using (HttpContent content = response.Content)
            {
                return await content.ReadAsStreamAsync();
            }
        }

	    public async void UploadString(string url, string data) {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("", data)
            });
		    await PostAsync(url, content);
	    }

	    public async Task<Stream> OpenRead(string url)
        {
            using (HttpResponseMessage response = await GetAsync(url))
            using (HttpContent content = response.Content) {
	            ResponseHeaders = response.Headers;
	            return await content.ReadAsStreamAsync();
            }
        }

	    public async void DownloadFile(string url, string path) {
            using (HttpResponseMessage response = await GetAsync(url))
            using (HttpContent content = response.Content)
            {
                ResponseHeaders = response.Headers;
                byte[] result = await content.ReadAsByteArrayAsync();
                
                if (result != null)
                {
                    File.WriteAllBytes(path, result);
                }
            }
        }

	    public async Task<string> DownloadString(string url)
        {
            using (HttpResponseMessage response = await GetAsync(url))
            using (HttpContent content = response.Content)
            {
                ResponseHeaders = response.Headers;
                string str = await content.ReadAsStringAsync();

                return str;
            }
        }

	    public HttpResponseHeaders ResponseHeaders { get; set; }
    }
}
