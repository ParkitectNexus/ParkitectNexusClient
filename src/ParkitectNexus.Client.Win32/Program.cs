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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using CommandLine;
using ParkitectNexus.Client.Base;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Reporting;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using Xwt;

namespace ParkitectNexus.Client.Win32
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Look and see if this is the only running instance of the client.
            var procCount =
#if !DEBUG
                Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName)
                    .Count(p => p.MainModule.FileName == Process.GetCurrentProcess().MainModule.FileName);
#else
                Process.GetProcesses().Count(p => p.ProcessName.Contains("ParkitectNexus"));
#endif

            if (procCount == 1)
            {
                // No matter if the application crashes, we must release the mutex when the app closes. Wrap the app
                // logic in a try-finally block.
#if !DEBUG
                try
                {
#endif
                    // Initialize the structure map container.
                    var registry = ObjectFactory.ConfigureStructureMap();
                    registry.IncludeRegistry(new PresenterRegistry());
                    registry.For<IApp>().Singleton().Use<App>();
                    ObjectFactory.SetUpContainer(registry);


                    // Create the form and run its message loop. If arguments were specified, process them within the
                    // form.
                    var presenterFactory = ObjectFactory.GetInstance<IPresenterFactory>();
                    var app = presenterFactory.InstantiatePresenter<App>();
                    if (!app.Initialize(ToolkitType.Wpf))
                        return;

                    ParkitectNexusProtocol.Install(ObjectFactory.GetInstance<ILogger>());

                    if (args.Any())
                    {
                        var options = new AppCommandLineOptions();
                        Parser.Default.ParseArguments(args, options);

                        if (options.Url != null)
                        {
                            NexusUrl url;
                            if (NexusUrl.TryParse(options.Url, out url))
                                app.HandleUrl(url);
                        }
                    }

                    app.Run();
#if !DEBUG
                }
                catch (Exception e)
                {
                    // Report crash to the server.
                    var crashReporterFactory = ObjectFactory.GetInstance<ICrashReporterFactory>();
                    crashReporterFactory.Report("global", e);

                    // Write the error to the log file.
                    var log = ObjectFactory.GetInstance<ILogger>();
                    log?.WriteLine("Application crashed!", LogLevel.Fatal);
                    log?.WriteException(e);
                }
#endif
                return;
            }

            // If any arguments are set, pass these on to our main application instance.
            if (args.Any())
            {
                var attempts = 0;
                do
                {
                    try
                    {
                        // Write the specified arguments to a temporary ipc.dat file.
                        using (var fileStream = File.OpenWrite(Path.Combine(AppData.Path, "ipc.dat")))
                        using (var streamWriter = new StreamWriter(fileStream))
                        {
                            var options = new AppCommandLineOptions();
                            Parser.Default.ParseArguments(args, options);

                            if (options.Url != null)
                                streamWriter.WriteLine(options.Url);
                        }

                        return;
                    }
                    catch (IOException)
                    {
                        // If storing the arguments fails, we're in trouble. Let's try it again in a few.
                        Thread.Sleep(500);
                        attempts++;
                    }
                } while (attempts < 5); // Limit to 5 attempts.
            }
        }
    }
}