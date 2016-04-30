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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Reporting
{
    [JsonObject(MemberSerialization.OptIn)]
    public class WindowsCrashReport
    {
        private readonly ILogger _logger;
        private readonly IParkitect _parkitect;

        public WindowsCrashReport(IParkitect parkitect, string action, Exception exception, ILogger logger)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            _logger = logger;
            _parkitect = parkitect;
            Action = action;
            Exception = exception;
        }

        [JsonProperty]
        public string Action { get; }

        [JsonProperty]
        public Exception Exception { get; }

        [JsonProperty]
        public string OS
            =>
                (from x in
                    new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get()
                        .OfType<ManagementObject>()
                    select x.GetPropertyValue("Caption")).FirstOrDefault()?.ToString() ?? "Unknown (Windows)";

        [JsonProperty]
        public int ProcessBits => IntPtr.Size*8;


        [JsonProperty]
        public int OSBits => InternalCheckIsWow64() ? 64 : 32;

        [JsonProperty]
        public MemStatusText MemoryStatus => new MemStatusText(InternalGlobalMemoryStatusEx());

        [JsonProperty]
        public IEnumerable<string> Mods
        {
            get
            {
                try
                {
                    return _parkitect.Assets[AssetType.Mod].OfType<ModAsset>().Select(
                        m => $"{m}(Enabled: ???, Directory: {m.InstallationPath})").ToArray();
                }
                catch
                {
                    return new[] {"Failed to list"};
                }
            }
        }

        [JsonProperty]
        public string Log
        {
            get
            {
                try
                {
                    if (!_logger.IsOpened)
                        return "not opened";

                    var path = _logger.LoggingPath;
                    _logger.Close();
                    var result = File.ReadAllText(path);
                    _logger.Open(path);
                    return result;
                }
                catch (Exception e)
                {
                    return "failed to open: " + e.Message;
                }
            }
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool wow64Process);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalMemoryStatusEx([In, Out] MemStatus lpBuffer);

        private static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (var p = Process.GetCurrentProcess())
                {
                    bool retVal;
                    return IsWow64Process(p.Handle, out retVal) && retVal;
                }
            }
            return false;
        }

        private static MemStatus InternalGlobalMemoryStatusEx()
        {
            var stat = new MemStatus();
            GlobalMemoryStatusEx(stat);
            return stat;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MemStatus
        {
            public readonly uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullAvailExtendedVirtual;
            public ulong ullAvailPageFile;
            public ulong ullAvailPhys;
            public ulong ullAvailVirtual;
            public ulong ullTotalPageFile;
            public ulong ullTotalPhys;
            public ulong ullTotalVirtual;

            public MemStatus()
            {
                dwLength = (uint) Marshal.SizeOf(typeof (MemStatus));
            }
        }

        [JsonObject]
        public class MemStatusText
        {
            public MemStatusText(MemStatus status)
            {
                Length = GetSizeReadable(status.dwLength);
                MemoryLoad = GetSizeReadable(status.dwMemoryLoad);
                TotalPhysical = GetSizeReadable(status.ullTotalPhys);
                AvailPhysical = GetSizeReadable(status.ullAvailPhys);
                TotalPageFile = GetSizeReadable(status.ullTotalPageFile);
                AvailPageFile = GetSizeReadable(status.ullAvailPageFile);
                TotalVirtual = GetSizeReadable(status.ullTotalVirtual);
                AvailVirtual = GetSizeReadable(status.ullAvailVirtual);
                AvailExtendedVirtual = GetSizeReadable(status.ullAvailExtendedVirtual);
            }

            public string Length { get; }

            public string MemoryLoad { get; }

            public string TotalPhysical { get; }

            public string AvailPhysical { get; }

            public string TotalPageFile { get; }

            public string AvailPageFile { get; }

            public string TotalVirtual { get; }

            public string AvailVirtual { get; }

            public string AvailExtendedVirtual { get; }

            private static string GetSizeReadable(ulong input)
            {
                double readable;
                string suffix;
                if (input >= 0x1000000000000000) // Exabyte
                {
                    suffix = "EB";
                    readable = input >> 50;
                }
                else if (input >= 0x4000000000000) // Petabyte
                {
                    suffix = "PB";
                    readable = input >> 40;
                }
                else if (input >= 0x10000000000) // Terabyte
                {
                    suffix = "TB";
                    readable = input >> 30;
                }
                else if (input >= 0x40000000) // Gigabyte
                {
                    suffix = "GB";
                    readable = input >> 20;
                }
                else if (input >= 0x100000) // Megabyte
                {
                    suffix = "MB";
                    readable = input >> 10;
                }
                else if (input >= 0x400) // Kilobyte
                {
                    suffix = "KB";
                    readable = input;
                }
                else
                {
                    return input.ToString("0 B"); // Byte
                }
                readable = readable/1024;

                return readable.ToString("0.### ") + suffix;
            }
        }
    }
}