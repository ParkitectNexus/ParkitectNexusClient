// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Presenter;
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
            MessageBox.Show("start with args " + string.Join(" ", args));

            // Find another instance of the process.
            var currentProcess = Process.GetCurrentProcess();

            var runningPath = Assembly.GetEntryAssembly().Location;

            MessageBox.Show("lookin for processes");
            Process foundProcess = null;
            foreach (var p in Process.GetProcesses())
            {
                if (p.ProcessName.Replace(".vshost", "") == currentProcess.ProcessName.Replace(".vshost", ""))
                {
                    MessageBox.Show($"checkin {p}");
                    if (PathUtility.ArePathsEqual(runningPath, p.MainModule.FileName))
                    {
                        MessageBox.Show($"check id {p.Id} != {currentProcess.Id}");
                        if (p.Id != currentProcess.Id)
                        {
                            MessageBox.Show("yup");
                            foundProcess = p;
                            break;
                        }
                    }
                    MessageBox.Show("nop");
                }
            }

            MessageBox.Show("Found process? " + (foundProcess != null));

            if (foundProcess == null)
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
                return;
            }

            // If any arguments are set, pass these on to our main application instance.
            if (args.Any())
            {
                MessageBox.Show("sent args A");
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
                        NativeMethods.SendNotifyMessage((IntPtr) NativeMethods.HWND_BROADCAST,
                            (uint) NativeMethods.WM_GIVEFOCUS, 1, 0);

                        MessageBox.Show("sent args B");
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

            // Send a broadcast to all open applications with our custom WM_GIVEFOCUS message. This message
            // will be picked up by our main instance of the application.
            NativeMethods.SendNotifyMessage((IntPtr)NativeMethods.HWND_BROADCAST,
                (uint)NativeMethods.WM_GIVEFOCUS, 1, 0);
        }
    }
}
