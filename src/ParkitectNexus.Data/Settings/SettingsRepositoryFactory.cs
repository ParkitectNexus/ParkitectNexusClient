// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze


namespace ParkitectNexus.Data.Settings
{
    public class SettingsRepositoryFactory : ISettingsRepositoryFactory
    {
        ISettingsRepository<T> ISettingsRepositoryFactory.Repository<T>()
        {
            return ObjectFactory.GetInstance<ISettingsRepository<T>>();
        }
    }
}
