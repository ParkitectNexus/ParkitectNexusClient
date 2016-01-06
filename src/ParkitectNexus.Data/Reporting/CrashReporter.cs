// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using Newtonsoft.Json;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Web;

namespace ParkitectNexus.Data.Reporting
{
    public static class CrashReporter
    {
        public static void Report(string action, IParkitect parkitect, IParkitectNexusWebsite website,
            Exception exception)
        {
            if (exception == null) return;

            try
            {
                var data = JsonConvert.SerializeObject(Generate(action, parkitect, exception));

                using (var client = new ParkitectNexusWebClient())
                {
                    client.UploadString(website.ResolveUrl("report/crash", "client"), data);
                }
            }
            catch
            {
            }
        }

        private static object Generate(string action, IParkitect parkitect, Exception exception)
        {
            try
            {
                var os = OperatingSystems.GetOperatingSystem();
                switch (os)
                {
                    case SupportedOperatingSystem.Windows:
                        return new WindowsCrashReport(parkitect, action, exception);
                    case SupportedOperatingSystem.MacOSX:
                        return new MacOSXCrashReport(parkitect, action, exception);
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