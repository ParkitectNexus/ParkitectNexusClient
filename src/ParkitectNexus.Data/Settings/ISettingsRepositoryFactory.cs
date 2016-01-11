// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

namespace ParkitectNexus.Data.Settings
{
    public interface ISettingsRepositoryFactory
    {
        ISettingsRepository<T> Repository<T>();
    }
}
