// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using ParkitectNexus.Data.Assets;
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
            For<ILocalAssetRepository>().Singleton().Use<LocalAssetRepository>();

            switch (OperatingSystem.Detect())
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