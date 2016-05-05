// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Drawing;
using ParkitectNexus.Data.Assets.CachedData;
using ParkitectNexus.Data.Assets.Meta;

namespace ParkitectNexus.Data.Assets
{
    public class BlueprintAsset : Asset
    {
        public BlueprintAsset(string path, IAssetMetadata metadata, IAssetCachedData cachedData)
            : base(path, metadata, cachedData, AssetType.Blueprint)
        {
        }

        #region Overrides of Asset

        public override Image GetImage()
        {
            using (var stream = Open())
            {
                if (stream == null)
                    return null;

                using (var image = Image.FromStream(stream))
                {
                    var result = new Bitmap(image.Width, image.Height);
                    using (var graphics = Graphics.FromImage(result))
                        graphics.DrawImage(image, 0, 0);

                    return result;
                }
            }
        }

        #endregion
    }
}