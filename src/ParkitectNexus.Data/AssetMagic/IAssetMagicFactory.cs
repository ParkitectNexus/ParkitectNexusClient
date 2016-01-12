using ParkitectNexus.AssetMagic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.AssetMagic
{
    public interface IAssetMagicFactory
    {
        /// <summary>
        /// returns an array of blueprint names
        /// </summary>
        string[] BlueprintNames { get; }
        /// <summary>
        /// returns an array of save game names
        /// </summary>
        string[] SavegameNames { get; }

        /// <summary>
        /// Get all the blueprints
        /// </summary>
        /// <returns>blueprints</returns>
        BlueprintContainer[] GetAllBlueprints();

        /// <summary>
        /// retrieves a single blueprint based on the name
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>BlueprintContainer</returns>
        BlueprintContainer GetBlueprint(string fileName);

        /// <summary>
        /// Gets a list of saved games
        /// </summary>
        /// <returns>ISavegame</returns>
        ISavegame[] GetAllSaveGames();
        /// <summary>
        /// retrieves a single saved game
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        ISavegame GetSaveGame(string fileName);
    }
}
