// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Data.Game.MacOSX;
using ParkitectNexus.Data.Game.Windows;
using StructureMap;

namespace ParkitectNexus.Data.Game
{
    public class GameRegistry : Registry
    {
        private readonly IOperatingSystem _operatingSystem = new OperatingSystems();

        public GameRegistry()
        {
            For<IParkitectMod>().Use<ParkitectMod>();
            For<IParkitectAsset>().Use<ParkitectAsset>();

            switch (_operatingSystem.GetOperatingSystem())
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
            }
        }
    }
}
