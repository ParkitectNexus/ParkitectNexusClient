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
using System.Runtime.InteropServices;

namespace ParkitectNexus.Data.Utilities
{
    public static class OperatingSystem
    {
        public static SupportedOperatingSystem Detect()
        {
            var platform = Environment.OSVersion.Platform;
            switch (platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    return SupportedOperatingSystem.Windows;
                case PlatformID.MacOSX:
                    return SupportedOperatingSystem.MacOSX;
                case PlatformID.Unix:
                    return IsUnixMacOSXPlatform()
                        ? SupportedOperatingSystem.MacOSX
                        : SupportedOperatingSystem.Linux;
                default:
                    throw new ApplicationException("unsupported platform " + platform);
            }
        }

        [DllImport("libc")]
        private static extern int uname(IntPtr buf);

        private static bool IsUnixMacOSXPlatform()
        {
            var buf = IntPtr.Zero;
            try
            {
                buf = Marshal.AllocHGlobal(8192);
                // This is a hacktastic way of getting sysname from uname ()
                if (uname(buf) == 0)
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
                    Marshal.FreeHGlobal(buf);
            }

            return false;
        }
    }
}