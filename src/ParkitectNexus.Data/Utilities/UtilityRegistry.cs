// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using StructureMap;

namespace ParkitectNexus.Data.Utilities
{
    public class UtilityRegistry : Registry
    {
        public UtilityRegistry()
        {
            //only a single instance of the logger is needed
            For<ILogger>().Use<Logger>().Singleton();
        }
    }
}