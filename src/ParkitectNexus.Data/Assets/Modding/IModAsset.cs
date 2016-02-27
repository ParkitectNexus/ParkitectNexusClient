// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.IO;

namespace ParkitectNexus.Data.Assets.Modding
{
    public interface IModAsset : IAsset
    {
        ModInformation Information { get; }

        string Tag { get; }

        string Repository { get; }

        StreamWriter OpenLogFile();
        void SaveInformation();
    }
}
