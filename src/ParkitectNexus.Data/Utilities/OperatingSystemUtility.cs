// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Runtime.InteropServices;

namespace ParkitectNexus.Data.Utilities
{
    public static class OperatingSystemUtility
	{
		[DllImport("libc")] 
		private static extern int uname(IntPtr buf); 

		private static bool IsRunningOnMac()
		{ 
			IntPtr buf = IntPtr.Zero; 
			try
			{ 
				buf = Marshal.AllocHGlobal (8192); 
				// This is a hacktastic way of getting sysname from uname () 
				if (uname (buf) == 0)
				{ 
					string os = Marshal.PtrToStringAnsi (buf); 
					if (os == "Darwin")
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
			case PlatformID.Unix:
				if (IsRunningOnMac ())
					return SupportedOperatingSystem.MacOSX;
				break;
            }

			throw new ApplicationException("unsupported platform " + Environment.OSVersion.Platform);
        }
    }
}