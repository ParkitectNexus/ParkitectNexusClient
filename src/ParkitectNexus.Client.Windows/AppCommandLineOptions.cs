﻿// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using CommandLine;

namespace ParkitectNexus.Client.Windows
{
    public class AppCommandLineOptions
    {
        [Option("url")]
        public string Url { get; set; }

        [Option('l', "launch")]
        public bool Launch { get; set; }
    }
}
