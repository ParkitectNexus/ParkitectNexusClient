// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using Newtonsoft.Json;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.Client;

namespace ParkitectNexus.Data.Reporting
{
    public class CrashReporterFactory : ICrashReporterFactory
    {
        private readonly ILogger _logger;
        private readonly IOperatingSystem _operatingSystem;
        private readonly IParkitect _parkitect;
        private readonly IParkitectNexusWebFactory _webFactory;
        private readonly IParkitectNexusWebsite _website;

        public CrashReporterFactory(IParkitectNexusWebFactory webFactory, IParkitectNexusWebsite website,
            IParkitect parkitect, IOperatingSystem operatingSystem, ILogger logger)
        {
            _website = website;
            _parkitect = parkitect;
            _webFactory = webFactory;
            _operatingSystem = operatingSystem;
            _logger = logger;
        }

        public void Report(string action, Exception exception)
        {
            if (exception == null) return;

            try
            {
                var data = JsonConvert.SerializeObject(Generate(action, _parkitect, exception));

                using (var client = _webFactory.CreateWebClient())
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
                var os = _operatingSystem.Detect();
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
