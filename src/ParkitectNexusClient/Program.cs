// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using ParkitectNexus.Data;
using StructureMap;
using StructureMap.Pipeline;

namespace ParkitectNexus.Client
{
    /// <summary>
    ///     Represents the entry point class for the application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            //configure map
            Registry registry = ObjectFactory.ConfigureStructureMap();
            registry.For<IApp>().Use<App>();
            ObjectFactory.SetUpContainer(registry);

            var app = ObjectFactory.Container.GetInstance<IApp>(
                new ExplicitArguments(new Dictionary<string, object>() {["args"] = args}));
            app.Run();
        }
    }
}
