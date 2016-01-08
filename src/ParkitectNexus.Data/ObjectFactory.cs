using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Reporting;
using ParkitectNexus.Data.Settings;
using ParkitectNexus.Data.Web;
using StructureMap;


namespace ParkitectNexus.Data
{
    public static class ObjectFactory
    {
        public static IContainer Container;
        public static Registry ConfigureStructureMap()
        {
            var registry = new Registry();
           
            registry.IncludeRegistry<WebRegistry>();
            registry.IncludeRegistry<GameRegistry>();
            registry.IncludeRegistry<PresenterRegistry>();

            //create operating system
            registry.For<IOperatingSystem>().Use<OperatingSystems>();

            //repository settings
            registry.For(typeof(IRepository<>)).Use(typeof(Repository<>));
            registry.For<IRepositoryFactory>().Use<RepositoryFactory>();

            registry.For<ICrashReporterFactory>().Use<CrashReporterFactory>();

            registry.For<IOperatingSystem>().Use<OperatingSystems>();
            registry.For<IPathResolver>().Use<PathResolver>();


            return registry;
           
        }

        public static void SetUpContainer(Registry registry)
        {
            Container = new Container(registry);
            
        }

    }
}
