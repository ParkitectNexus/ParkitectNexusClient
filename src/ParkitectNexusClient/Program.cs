// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using ParkitectNexus.Data;
using StructureMap;

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
            registry.For<IClient>().Use<Client>();
            ObjectFactory.SetUpContainer(registry);

            ObjectFactory.Container.GetInstance<IClient>();
        }
    }
}