// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
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
            var operatingSystem = new OperatingSystem();
            For<IParkitectMod>().Use<ParkitectMod>();
            For<IParkitectDownloadedAsset>().Use<ParkitectDownloadedAsset>();

            switch (operatingSystem.Detect())
            {
                case SupportedOperatingSystem.Linux:
                    For<IParkitect>().Use<LinuxParkitect>();
                    break;
                case SupportedOperatingSystem.MacOSX:
                    For<IParkitect>().Use<MacOSXParkitect>();
                    break;
                case SupportedOperatingSystem.Windows:
                    For<IParkitect>().Use<WindowsParkitect>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("operating system not supported");
            }
        }
    }
}
