// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

namespace ParkitectNexus.Data.Caching
{
    public interface ICacheManager
    {
        T GetItem<T>(string name) where T : class;
        void SetItem<T>(string name, T item) where T : class;
    }
}
