// ParkitectNexusInstaller
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Reflection;

namespace ParkitectNexusInstaller
{
    internal static class EnumUtil
    {
        public static T GetCustomAttribute<T>(this Enum @enum) where T : Attribute
        {
            if (@enum == null) throw new ArgumentNullException(nameof(@enum));
            var type = @enum.GetType();

            var memInfo = type.GetMember(@enum.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                var attribute = memInfo[0].GetCustomAttribute<T>();
                return attribute;
            }

            return null;
        }
    }
}