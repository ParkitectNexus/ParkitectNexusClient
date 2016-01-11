// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Windows.Forms;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Presenter;
using StructureMap;

namespace ParkitectNexus.Client.Windows
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //configure map
            Registry registry = ObjectFactory.ConfigureStructureMap();
            registry.IncludeRegistry(new PresenterRegistry());
            ObjectFactory.SetUpContainer(registry);

            var presenterFactory = ObjectFactory.Container.GetInstance<IPresenterFactory>();
            Application.Run(presenterFactory.InstantiatePresenter<MainForm>());
        }
    }
}