// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Data.Settings
{
    public interface ISettingsRepository<T> : IDisposable
    {
        T Model { get; }
        void Load();
        void Save();
    }
}
