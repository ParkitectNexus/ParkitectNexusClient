using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using ParkitectNexus.AssetMagic.Writers;
using ParkitectNexus.AssetMagic.Readers;

namespace ParkitectNexus.Data.AssetMagic
{
    public class AssetMagicRegistry : Registry
    {
        public AssetMagicRegistry()
        {
            For<IBlueprintWriter>().Use<BlueprintWriter>();
            For<ISavegameWriter>().Use<SavegameWriter>();

            For<IBlueprintReader>().Use<BlueprintReader>();
            For<ISavegameReader>().Use<SavegameReader>();

            For<IAssetMagicFactory>().Use<AssetMagicFactory>();
        }
    }
}
