// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;

namespace ParkitectNexus.Data.Utilities
{
    public static class OperatingSystemUtility
    {
        public static SupportedOperatingSystem GetOperatingSystem()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    return SupportedOperatingSystem.Windows;
                case PlatformID.MacOSX:
                    return SupportedOperatingSystem.MacOSX;
                default:
                    throw new ApplicationException("unsupported platform");
            }
        }
    }
}