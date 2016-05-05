// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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