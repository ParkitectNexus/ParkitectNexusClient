

using System;

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
