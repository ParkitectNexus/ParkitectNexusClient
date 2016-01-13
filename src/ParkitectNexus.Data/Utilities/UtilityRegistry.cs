using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Utilities
{
    public class UtilityRegistry : Registry
    {
        public UtilityRegistry()
        {
            //operating system
            For<IOperatingSystem>().Use<OperatingSystem>();

            //create operating system
            For<IOperatingSystem>().Use<OperatingSystem>();

            //only a single instance of the logger is needed
            For<ILogger>().Use<Logger>().Singleton();

        }
    }
}
