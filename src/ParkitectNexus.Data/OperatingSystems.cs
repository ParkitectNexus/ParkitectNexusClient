// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Runtime.InteropServices;

namespace ParkitectNexus.Data
{
    public class OperatingSystems : IOperatingSystem
    {
        [DllImport("libc")] 
        private static extern int uname(IntPtr buf); 

        private static bool IsUnixMacOSXPlatform()
        { 
            var buf = IntPtr.Zero; 
            try
            { 
                buf = Marshal.AllocHGlobal (8192); 
                // This is a hacktastic way of getting sysname from uname () 
                if (uname (buf) == 0)
                { 
                    if (Marshal.PtrToStringAnsi(buf) == "Darwin")
                        return true; 
                } 
            }
            catch
            {
            }
            finally
            { 
                if (buf != IntPtr.Zero)
                    Marshal.FreeHGlobal (buf); 
            }

            return false; 
        }

        public SupportedOperatingSystem GetOperatingSystem()
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
            case PlatformID.Unix:
                if (IsUnixMacOSXPlatform ())
                    return SupportedOperatingSystem.MacOSX;
                else
                    return SupportedOperatingSystem.Linux;
            }

            throw new ApplicationException("unsupported platform " + Environment.OSVersion.Platform);
        }
    }
}