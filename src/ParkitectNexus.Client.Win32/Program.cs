// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using ParkitectNexus.Client.Base;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Presenter;
using Xwt;

namespace ParkitectNexus.Client.Win32
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var registry = ObjectFactory.ConfigureStructureMap();
            registry.IncludeRegistry(new PresenterRegistry());
            ObjectFactory.SetUpContainer(registry);

            var presenterFactory = ObjectFactory.GetInstance<IPresenterFactory>();

            var app = presenterFactory.InstantiatePresenter<App>();
            app.Run(ToolkitType.Wpf);
        }
    }
}
