// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using StructureMap;

namespace ParkitectNexus.Data.Presenter
{
    public class PresenterRegistry : Registry
    {
        public PresenterRegistry()
        {
            Scan(x =>
            {
                x.IncludeNamespaceContainingType<IPresenter>();
                x.WithDefaultConventions();
            });

            For<IPresenterFactory>().Use<PresenterFactory>();
        }
    }
}