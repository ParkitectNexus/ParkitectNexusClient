// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;

namespace ParkitectNexus.Data.Game
{
    public interface IParkitectAssetDataCache<T> where T : new()
    {
        T GetCachedData(string path, Func<string, T> resolver);
        void ClearCachedData(IEnumerable<string> except);
        void Reload();
        void Save();
    }
}
