// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Threading.Tasks;

namespace ParkitectNexus.Data.Caching
{
    public interface ICacheManager
    {
        T GetItem<T>(string name) where T : class;
        T GetItemOrNew<T>(string name) where T : class;
        void SetItem<T>(string name, T item) where T : class;
        void Clear();
    }
}
