// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using CommandLine;

<<<<<<< HEAD
namespace ParkitectNexus.Client.Linux
=======
namespace ParkitectNexus.Client.Win32
>>>>>>> fe06d0ca5facea05edbf8cde2693cfac2016193c
{
    public class AppCommandLineOptions
    {
        [Option("url")]
        public string Url { get; set; }

        [Option('l', "launch")]
        public bool Launch { get; set; }
    }
}
