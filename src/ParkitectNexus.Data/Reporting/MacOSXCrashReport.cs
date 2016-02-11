// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Data.Reporting
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MacOSXCrashReport
    {
        private readonly ILogger _logger;
        private readonly IParkitect _parkitect;

        public MacOSXCrashReport(IParkitect parkitect, string action, Exception exception, ILogger logger)
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
        {
            get
            {
                try
                {
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "w_vers",
                            Arguments = "-productVersion",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            CreateNoWindow = true
                        }
                    };

                    string result = "";
                    process.Start();
                    while (!process.StandardOutput.EndOfStream)
                    {
                        result += process.StandardOutput.ReadLine();
                    }
                    return result;
                }
                catch
                {
                    return "Unknown (MacOSX)";
                }
            }
        }

        [JsonProperty]
        public int ProcessBits => IntPtr.Size*8;

        [JsonProperty]
        public IEnumerable<string> Mods
        {
            get
            {
                try
                {
                    return _parkitect.Assets[AssetType.Mod].OfType<ModAsset>().Select(
                        m => $"{m}(Enabled: ???, Directory: {m.InstallationPath})");
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
    }
}