﻿// ParkitectNexusClient
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

namespace ParkitectNexus.Data.Assets
{
    public static class AssetTypeUtil
    {
        public static AssetType Parse(string value)
        {
            if (value == null)
                return default(AssetType);

            AssetType parsed;
            if (Enum.TryParse(value, out parsed))
                return parsed;

            switch (value)
            {
                case "park":
                    return AssetType.Savegame;
                case "mod":
                    return AssetType.Mod;
                case "blueprint":
                    return AssetType.Blueprint;
                case "scenario":
                    return AssetType.Scenario;
                default:
                    return default(AssetType);
            }
        }
    }
}
