// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

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
        private readonly ILogger _logger;
        private readonly IParkitect _parkitect;
        private readonly INexusWebClientFactory _webClientFactory;
        private readonly IWebsite _website;

        public CrashReporterFactory(INexusWebClientFactory webClientFactory, IWebsite website,
            IParkitect parkitect, ILogger logger)
        {
            _website = website;
            _parkitect = parkitect;
            _webClientFactory = webClientFactory;
            _logger = logger;
        }

        public void Report(string action, Exception exception)
        {
            if (exception == null) return;

            try
            {
                var data = JsonConvert.SerializeObject(Generate(action, _parkitect, exception));

                using (var client = _webClientFactory.CreateWebClient())
                {
                    client.UploadString(_website.ResolveUrl("report/crash", "client"), data);
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
                        return new WindowsCrashReport(parkitect, action, exception, _logger);
                    case SupportedOperatingSystem.MacOSX:
                        return new MacOSXCrashReport(parkitect, action, exception, _logger);
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