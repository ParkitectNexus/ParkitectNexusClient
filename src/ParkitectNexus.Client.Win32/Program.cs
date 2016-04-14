// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
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
            // Create a mutex which is held while the app is open. If the app is started and the mutex can't be awaited,
            // assume the app is already running and broadcast a WM_GIVEFICUS message.
            bool mutexIsNew;
            var mutex = new Mutex(false, "com.ParkitectNexus.Client", out mutexIsNew);

            // Look and see if this is the only running instance of the client.
            if (mutexIsNew)
            {
                // No matter if the application crashes, we must release the mutex when the app closes. Wrap the app
                // logic in a try-finally block.
                try
                {
                    // Increase maximum threads.
                    ThreadPool.SetMaxThreads(16, 16);

                    // Initialize the structure map container.
                    var registry = ObjectFactory.ConfigureStructureMap();
                    registry.IncludeRegistry(new PresenterRegistry());
                    ObjectFactory.SetUpContainer(registry);


                    // Create the form and run its message loop. If arguments were specified, process them within the
                    // form.
                    var presenterFactory = ObjectFactory.GetInstance<IPresenterFactory>();
                    var app = presenterFactory.InstantiatePresenter<App>();
                    app.Initialize(ToolkitType.Wpf);

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

            GC.KeepAlive(mutex);
        }
    }
}
