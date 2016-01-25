// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Assets
{
    public interface IAssetsRepository
    {
        IEnumerable<Asset> GetBlueprints();
        IEnumerable<Asset> GetSavegames();
        int GetBlueprintsCount();
        int GetSavegamesCount();
    }
}
