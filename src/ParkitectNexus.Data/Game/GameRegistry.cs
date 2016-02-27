// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using ParkitectNexus.Data.Game.Linux;
using ParkitectNexus.Data.Game.MacOSX;
using ParkitectNexus.Data.Game.Windows;
using StructureMap;
using OperatingSystem = ParkitectNexus.Data.Utilities.OperatingSystem;

namespace ParkitectNexus.Data.Game
{
    public class GameRegistry : Registry
    {
        public GameRegistry()
        {
            For<IDownloadedAsset>().Use<DownloadedAsset>();

            switch (OperatingSystem.Detect())
            {
                case SupportedOperatingSystem.Linux:
                    For<IParkitectPaths>().Use<LinuxParkitectPaths>();
                    For<IParkitect>().Singleton().Use<LinuxParkitect>();
                    break;
                case SupportedOperatingSystem.MacOSX:
                    For<IParkitectPaths>().Use<MacOSXParkitectPaths>();
                    For<IParkitect>().Singleton().Use<MacOSXParkitect>();
                    break;
                case SupportedOperatingSystem.Windows:
                    For<IParkitectPaths>().Use<WindowsParkitectPaths>();
                    For<IParkitect>().Singleton().Use<WindowsParkitect>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("operating system not supported");
            }
        }
    }
}
