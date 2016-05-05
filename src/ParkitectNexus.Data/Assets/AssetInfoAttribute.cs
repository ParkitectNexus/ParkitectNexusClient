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

namespace ParkitectNexus.Data.Assets
{
    /// <summary>
    ///     Provides information about a parkitect asset.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class AssetInfoAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AssetInfoAttribute" /> class.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public AssetInfoAttribute(string contentType)
        {
            ContentType = contentType;
        }

        /// <summary>
        ///     Gets the type of the content provided when an asset of this type is being downloaded.
        /// </summary>
        public string ContentType { get; }
    }
}