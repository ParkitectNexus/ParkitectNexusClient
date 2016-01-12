using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkitectNexus.AssetMagic;
using ParkitectNexus.Data.Game;
using ParkitectNexus.AssetMagic.Writers;
using ParkitectNexus.AssetMagic.Readers;
using System.IO;
using System.Drawing;

namespace ParkitectNexus.Data.AssetMagic
{
    public class AssetMagicFactory : IAssetMagicFactory
    {
        private readonly IBlueprintReader _blueprintReader;
        private readonly IBlueprintWriter _blueprintWriter;
        private readonly ISavegameReader _savegameReader;
        private readonly ISavegameWriter _savegameWriter;
        private readonly IParkitect _parkitect;

        public AssetMagicFactory(IParkitect parkitect, IBlueprintWriter blueprintWriter, ISavegameWriter savegameWriter, IBlueprintReader blueprintReader, ISavegameReader savegameReader)
        {
            _parkitect = parkitect;
            _blueprintReader = blueprintReader;
            _blueprintWriter = blueprintWriter;
            _savegameReader = savegameReader;
            _savegameWriter = savegameWriter;
        }

        public string[] BlueprintNames
        {
            get
            {

                return Directory.GetFiles(_parkitect.Paths.GetPathInSavesFolder("Saves/Blueprints", true), "*.png").Select(x => Path.GetFileName(x)).ToArray();

            }
        }

        public string[] SavegameNames
        {
            get
            {

               return Directory.GetFiles(_parkitect.Paths.GetPathInSavesFolder("Saves/Savegames", true), "*.txt").Select(x => Path.GetFileName(x)).ToArray();

            }
        }

        public BlueprintContainer[] GetAllBlueprints()
        {
            var blueprintPath = Directory.GetFiles(_parkitect.Paths.GetPathInSavesFolder("Saves/Blueprints", true), "*.png");
            List<BlueprintContainer> blueprints = new List<BlueprintContainer>();
            foreach ( var file in blueprintPath)
            {
                using (var bitmap = (Bitmap)Image.FromFile(file))
                {
                    blueprints.Add(new BlueprintContainer() { blueprint = _blueprintReader.Read(bitmap),image = bitmap});
                }
            }
            return blueprints.ToArray();
        }

        public BlueprintContainer GetBlueprint(string blueprintName)
        {
            string path = Path.Combine(_parkitect.Paths.GetPathInSavesFolder("Saves/Blueprints", true), blueprintName + ".png");
            if(Directory.Exists(path))
            {
                using (var bitmap = (Bitmap)Image.FromFile(path))
                {
                    return new BlueprintContainer() { blueprint = _blueprintReader.Read(bitmap), image = bitmap };
                }
            }
            return null;
        }

        public ISavegame[] GetAllSaveGames()
        {
            var saveGamepath = Directory.GetFiles(_parkitect.Paths.GetPathInSavesFolder("Saves/Savegames", true), "*.txt");
            List<ISavegame> savegames = new List<ISavegame>();
            foreach (var file in saveGamepath)
            {
                using (var stream = File.OpenRead(file))
                {
                    savegames.Add(_savegameReader.Deserialize(stream));
                }
            }
            return savegames.ToArray();
        }

        public ISavegame GetSaveGame(string savegameName)
        {
            string path = Path.Combine(_parkitect.Paths.GetPathInSavesFolder("Saves/Blueprints", true), savegameName + ".png");
            if (Directory.Exists(path))
            {
                using (var stream = File.OpenRead(path))
                {
                    return _savegameReader.Deserialize(stream);
                }
            }
            return null;
        }
    }
}
