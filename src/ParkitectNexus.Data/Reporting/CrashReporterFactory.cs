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
using Newtonsoft.Json;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.Client;
using OperatingSystem = ParkitectNexus.Data.Utilities.OperatingSystem;

namespace ParkitectNexus.Data.Reporting
{
    public class CrashReporterFactory : ICrashReporterFactory
    {
        private readonly ILogger _log;
        private readonly IParkitect _parkitect;
        private readonly INexusWebClientFactory _webClientFactory;
        private readonly IWebsite _website;

        public CrashReporterFactory(INexusWebClientFactory webClientFactory, IWebsite website,
            IParkitect parkitect, ILogger log)
        {
            _website = website;
            _parkitect = parkitect;
            _webClientFactory = webClientFactory;
            _log = log;
        }

        public void Report(string action, Exception exception)
        {
            if (exception == null) return;

            try
            {
                var data = JsonConvert.SerializeObject(Generate(action, _parkitect, exception));

                using (var client = _webClientFactory.CreateWebClient())
                {
                    _log.WriteLine("Crash report is being sent.");
                    client.UploadString(_website.ResolveUrl("report/crash", "client"), data);
                    _log.WriteLine("Crash report was sent.");
                }
            }
            catch
            {
            }
        }

        private object Generate(string action, IParkitect parkitect, Exception exception)
        {
            try
            {
                var os = OperatingSystem.Detect();
                switch (os)
                {
                    case SupportedOperatingSystem.Windows:
                        return new WindowsCrashReport(parkitect, action, exception, _log);
                    case SupportedOperatingSystem.MacOSX:
                        return new MacOSXCrashReport(parkitect, action, exception, _log);
                    case SupportedOperatingSystem.Linux:
                        return new LinuxCrashReport(parkitect, action, exception, _log);
                    default:
                        throw new Exception("unsupported operating system " + os);
                }
            }
            catch (Exception e)
            {
                return new FailedCrashReport(action, exception, e);
            }
        }
    }
}