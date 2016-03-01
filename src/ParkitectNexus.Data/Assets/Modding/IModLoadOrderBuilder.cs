// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Collections.Generic;

namespace ParkitectNexus.Data.Assets.Modding
{
    public interface IModLoadOrderBuilder
    {
        IEnumerable<IModAsset> Build();
        void BuildAndStore();
    }
}
