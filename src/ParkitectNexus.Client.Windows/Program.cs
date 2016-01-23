// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using System.Linq;
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
            var mutex = new Mutex(false, "com.ParkitectNexus.Client");

            if (mutex.WaitOne(0, true))
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    //configure map
                    var registry = ObjectFactory.ConfigureStructureMap();
                    registry.IncludeRegistry(new PresenterRegistry());
                    ObjectFactory.SetUpContainer(registry);

                    var presenterFactory = ObjectFactory.GetInstance<IPresenterFactory>();
                    var form = presenterFactory.InstantiatePresenter<MainForm>();

                    if (args.Any())
                        form.ProcessArguments(args);

                    Application.Run(form);
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
            else
            {
                var ipc = 0;

                if (args.Any())
                {
                    try
                    {
                        using (var fileStream = File.OpenWrite(Path.Combine(AppData.Path, "ipc.dat")))
                        using (var streamWriter = new StreamWriter(fileStream))
                        {
                            foreach (var arg in args)
                                streamWriter.WriteLine(arg);
                        }

                        ipc = 1;
                    }
                    catch (IOException)
                    {
                        ipc = -1;
                    }
                }

                NativeMethods.SendNotifyMessage((IntPtr) NativeMethods.HWND_BROADCAST, (uint) NativeMethods.WM_GIVEFOCUS,
                    ipc, 0);
            }
        }
    }
}
