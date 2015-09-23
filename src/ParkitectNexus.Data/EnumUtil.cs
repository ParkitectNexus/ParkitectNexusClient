// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Reflection;

namespace ParkitectNexus.Data
{
    /// <summary>
    ///     Contains utility methods for enum values.
    /// </summary>
    public static class EnumUtil
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