using ParkitectNexus.AssetMagic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.AssetMagic
{
    public class BlueprintContainer
    {
        /// <summary>
        /// blueprint for parkitect
        /// </summary>
        public virtual IBlueprint blueprint { get; set; }
        /// <summary>
        /// the image associated with the blueprint
        /// </summary>
        public virtual Bitmap image { get; set; }
    }
}
