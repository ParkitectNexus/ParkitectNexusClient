// ParkitectNexusInstaller
// Copyright 2015 Parkitect, Tim Potze

using CommandLine;

namespace ParkitectNexusInstaller
{
    internal class CommandLineOptions
    {
        [Option('d', "download")]
        public string DownloadUrl { get; set; }

        [Option("set-installation-path")]
        public string SetInstallationPath { get; set; }
    }
}