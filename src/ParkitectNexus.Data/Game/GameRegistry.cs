using ParkitectNexus.Data.Base;
using ParkitectNexus.Data.Game.MacOSX;
using ParkitectNexus.Data.Game.Windows;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Game
{
    public class GameRegistry : Registry
    {
        private IOperatingSystem _operatingSystem = new OperatingSystems();

       public GameRegistry()
        {

            For<IParkitectMod>().Use<ParkitectMod>();
            For<IParkitectAsset>().Use<ParkitectAsset>();

            switch(_operatingSystem.GetOperatingSystem())
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
