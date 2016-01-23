// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Reporting;
using ParkitectNexus.Data.Settings;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using StructureMap;

namespace ParkitectNexus.Data
{
    public static class ObjectFactory
    {
        public static IContainer Container { get; private set; }

        public static Registry ConfigureStructureMap()
        {
            var registry = new Registry();

            registry.IncludeRegistry<WebRegistry>();
            registry.IncludeRegistry<GameRegistry>();
            registry.IncludeRegistry<PresenterRegistry>();
            registry.IncludeRegistry<UtilityRegistry>();

            //repository settings
            registry.For(typeof (ISettingsRepository<>)).Singleton().Use(typeof (SettingsRepository<>));

            //used to send crash reports
            registry.For<ICrashReporterFactory>().Use<CrashReporterFactory>();


            return registry;
        }

        public static T GetInstance<T>()
        {
            return Container.GetInstance<T>();
        }

        public static void SetUpContainer(Registry registry)
        {
            Container = new Container(registry);
        }
    }
}
