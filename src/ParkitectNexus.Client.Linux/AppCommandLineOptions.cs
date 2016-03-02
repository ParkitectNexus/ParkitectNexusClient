// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using CommandLine;

namespace  ParkitectNexus.Client.Linux
{
    public class AppCommandLineOptions
    {
        [Option("url")]
        public string Url { get; set; }
    }
}
