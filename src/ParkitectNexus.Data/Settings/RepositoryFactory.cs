// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze


namespace ParkitectNexus.Data.Settings
{
    public class RepositoryFactory : IRepositoryFactory
    {
        IRepository<T> IRepositoryFactory.Repository<T>()
        {
            return ObjectFactory.Container.GetInstance<IRepository<T>>();
        }
    }
}