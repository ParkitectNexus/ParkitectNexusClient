using ParkitectNexus.Data.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Web
{
    public interface IParkitectNexusUrl
    {

        /// <summary>
        ///     Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Gets the type of the asset.
        /// </summary>
        ParkitectAssetType AssetType { get; }

        /// <summary>
        ///     Gets the file hash.
        /// </summary>
        string FileHash { get; }
    }
}
