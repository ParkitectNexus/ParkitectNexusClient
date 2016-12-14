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

using System;
using System.Drawing;
using ParkitectNexus.Data.Assets.CachedData;
using ParkitectNexus.Data.Assets.Meta;

namespace ParkitectNexus.Data.Assets
{
    public class SavegameAsset : Asset
    {
        private readonly AssetWithImageCachedData _cachedData;

        public SavegameAsset(string path, IAssetMetadata metadata, AssetWithImageCachedData cachedData)
            : base(path, metadata, cachedData, AssetType.Savegame)
        {
            if (cachedData == null) throw new ArgumentNullException(nameof(cachedData));
            _cachedData = cachedData;
        }

        #region Overrides of Asset

        public override Image GetImage()
        {
            return _cachedData.GetImage();
        }

        #endregion
    }
}
