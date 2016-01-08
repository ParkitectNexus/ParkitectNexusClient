// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using System.Windows.Forms;
using CommandLine;
using ParkitectNexus.Client.Settings;
using ParkitectNexus.Client.Wizard;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Game.Windows;
using ParkitectNexus.Data.Reporting;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;

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
            StructureMap.Registry registry = ObjectFactory.ConfigureStructureMap();
            registry.For<IClient>().Use<Client>();
            ObjectFactory.SetUpContainer(registry);

            ObjectFactory.Container.GetInstance<IClient>();
            
        }

       
    }
}