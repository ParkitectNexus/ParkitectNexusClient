// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace ParkitectNexus.Data
{
    public static class CrashReporter
    {
        public static void Report(string action, Parkitect parkitect, ParkitectNexusWebsite website, Exception exception)
        {
            if (exception == null) return;

            try
            {
                var data = JsonConvert.SerializeObject(new CrashReport(parkitect, action, exception));

                using (var client = new ParkitectNexusWebClient())
                {
                    client.UploadString(website.ResolveUrl("report/crash", "client"), data);
                }
            }
            catch
            {
                
            }
        }

        [JsonObject(MemberSerialization.OptIn)]
        private class CrashReport
        {
            private readonly Parkitect _parkitect;

            public CrashReport(Parkitect parkitect, string action, Exception exception)
            {
                if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
                if (exception == null) throw new ArgumentNullException(nameof(exception));

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
                        select x.GetPropertyValue("Caption")).FirstOrDefault()?.ToString() ?? "Unknown";

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
                        return _parkitect.InstalledMods.Select(
                            m => $"{m}(Enabled: {m.IsEnabled}, Directory: {m.AssetBundlePrefix})");
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
                        if (!Utilities.Log.IsOpened)
                            return "not opened";

                        var path = Utilities.Log.LoggingPath;
                        Utilities.Log.Close();
                        var result = File.ReadAllText(path);
                        Utilities.Log.Open(path);
                        return result;
                    }
                    catch (Exception e)
                    {
                        return "failed to open: " + e.Message;
                    }
                }
            }

            [JsonProperty]
            public string MonoObjectInjectorLog
            {
                get
                {
                    try
                    {
                        var path = Path.Combine(AppData.Path, "MonoObjectInjector.log");
                        return File.Exists(path) ? File.ReadAllText(path) : null;
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
}