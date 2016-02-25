// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Assets.CachedData;
using ParkitectNexus.Data.Assets.Meta;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Authentication;
using ParkitectNexus.Data.Caching;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Reporting;
using ParkitectNexus.Data.Settings;
using ParkitectNexus.Data.Tasks;
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

            // repository settings
            registry.For(typeof (ISettingsRepository<>)).Singleton().Use(typeof (SettingsRepository<>));

            // used to send crash reports
            registry.For<ICrashReporterFactory>().Use<CrashReporterFactory>();

            // caching
            registry.For<ICacheManager>().Use<CacheManager>();

            registry.For<IQueueableTaskManager>().Singleton().Use<QueueableTaskManager>();

            registry.For<IAuthManager>().Singleton().Use<AuthManager>();

            registry.For<ILocalAssetRepository>().Use<LocalAssetRepository>();
            registry.For<IRemoteAssetRepository>().Use<RemoteAssetRepository>();
            registry.For<IAssetMetadataStorage>().Use<AssetMetadataStorage>();
            registry.For<IAssetCachedDataStorage>().Use<AssetCachedDataStorage>();
            registry.For<IModCompiler>().Use<ModCompiler>();
            registry.For<IModLoadOrderBuilder>().Use<ModLoadOrderBuilder>();
            registry.For<IAssetUpdatesManager>().Singleton().Use<AssetUpdatesManager>();

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
