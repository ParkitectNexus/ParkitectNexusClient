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
using System.Reflection;

namespace ParkitectNexus.Data.Utilities
{
    /// <summary>
    ///     Contains utility methods for enum values.
    /// </summary>
    public static class EnumUtility
    {
        /// <summary>
        ///     Gets the custom attribute of the enum value.
        /// </summary>
        /// <typeparam name="T">The type of the custom attribute</typeparam>
        /// <param name="enum">The enum.</param>
        /// <returns>The custom attribute found</returns>
        /// <exception cref="ArgumentNullException">Thrown if @enum is null.</exception>
        public static T GetCustomAttribute<T>(this Enum @enum) where T : Attribute
        {
            if (@enum == null) throw new ArgumentNullException(nameof(@enum));
            var type = @enum.GetType();

            // Get the member with the name of the specified enum value.
            var memberInfo = type.GetMember(@enum.ToString());

            // Find a custom attribute of the specified type within the member.
            return memberInfo != null && memberInfo.Length > 0 ? memberInfo[0].GetCustomAttribute<T>() : null;
        }
    }
}