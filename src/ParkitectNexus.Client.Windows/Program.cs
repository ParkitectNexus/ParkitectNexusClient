// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Reporting;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client.Windows
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
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

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    // Initialize the structure map container.
                    var registry = ObjectFactory.ConfigureStructureMap();
                    registry.IncludeRegistry(new PresenterRegistry());
                    ObjectFactory.SetUpContainer(registry);

                    // Create the form and run its message loop. If arguments were specified, process them within the
                    // form.
                    var form = ObjectFactory.GetInstance<IPresenterFactory>().InstantiatePresenter<MainForm>();
                    if (args.Any()) form.ProcessArguments(args);
                    Application.Run(form);
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

                    // Display a message to the user.
                    MessageBox.Show(
                        $"The ParkitectNexus Client crashed. :(\nWe've stored some details to the log file located at {log?.LoggingPath ?? "(unknown location)"}.");
                }
                finally
                {
                    // Release the mutex so other instances of the app can again be created.
                    // mutex.ReleaseMutex();
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
                            foreach (var arg in args)
                                streamWriter.WriteLine(arg);
                        }

                        // Send a broadcast to all open applications with our custom WM_GIVEFOCUS message. This message
                        // will be picked up by our main instance of the application which in turn will read the
                        // temporary ipc.dat file.
                        NativeMethods.SendNotifyMessage((IntPtr)NativeMethods.HWND_BROADCAST,
                            (uint)NativeMethods.WM_GIVEFOCUS, 1, 0);

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
