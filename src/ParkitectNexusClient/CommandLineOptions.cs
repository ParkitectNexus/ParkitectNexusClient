// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using CommandLine;

namespace ParkitectNexus.Client
{
    /// <summary>
    ///     Represents a collection of command line options which can be set with the execution of the application.
    /// </summary>
    internal class CommandLineOptions
    {
        /// <summary>
        ///     Gets the download URL of an asset file.
        /// </summary>
        [Option('d', "download")]
        public string DownloadUrl { get; set; }

        /// <summary>
        ///     Gets the set-installation-path option value. Should be path to the installation path of the game if set.
        /// </summary>
        [Option("set-installation-path")]
        public string SetInstallationPath { get; set; }

        /// <summary>
        ///     Gets a value indicating whether not to open the ParkitectNexus website when no download action is specified.
        /// </summary>
        [Option('s', "silent")]
        public bool Silent { get; set; }
    }
}