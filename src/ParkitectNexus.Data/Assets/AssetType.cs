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

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ParkitectNexus.Data.Assets
{
    /// <summary>
    ///     Contains parkitect asset types supported by the client.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AssetType
    {
        /// <summary>
        ///     A savegame.
        /// </summary>
        [AssetInfo(new[]{"text/plain","application/x-gzip"})]Savegame,

        /// <summary>
        ///     A blueprint.
        /// </summary>
        [AssetInfo(new[]{"image/png"})] Blueprint,

        /// <summary>
        ///     A mod.
        /// </summary>
        [AssetInfo(new[]{"application/zip"})] Mod,

        /// <summary>
        ///     A scenario.
        /// </summary>
        [AssetInfo(new[]{"application/x-gzip"})] Scenario,
    }
}
