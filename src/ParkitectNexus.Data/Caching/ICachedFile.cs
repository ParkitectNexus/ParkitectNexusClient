// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.IO;

namespace ParkitectNexus.Data.Caching
{
    public interface ICachedFile
    {
        bool Exists { get; }

        Stream Open(FileMode mode);
        void Delete();
    }
}
