// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Reflection;
using StructureMap;

namespace ParkitectNexus.Data.Presenter
{
    public class PresenterRegistry : Registry
    {
        public PresenterRegistry()
        {
            Scan(x =>
            {
                x.Assembly(Assembly.GetEntryAssembly());
                x.AddAllTypesOf<IPresenter>();
                x.WithDefaultConventions();
            });
            For<IPresenterFactory>().Use<PresenterFactory>();
        }
    }
}