using System;
using Gtk;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Reporting;
using ParkitectNexus.Data.Utilities;
using System.Threading;
using System.IO.Pipes;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using ParkitectNexus.Data.Tasks;
using Mono.Unix;

namespace ParkitectNexus.Client.Linux
{
    class MainClass
    {

        public static void Main (string[] args)
        {
            Application.Init();

            //configure map
            StructureMap.Registry registry = ObjectFactory.ConfigureStructureMap();
            ObjectFactory.SetUpContainer(registry);

            var presenterFactory = ObjectFactory.Container.GetInstance<IPresenterFactory>();
            var crashReport = ObjectFactory.Container.GetInstance<ICrashReporterFactory>();
            var logger = ObjectFactory.Container.GetInstance<ILogger>();
            logger.Open(System.IO.Path.Combine(AppData.Path, "ParkitectNexusLauncher.log"));

            string output = "";
            for (int x = 0; x < args.Length; x++)
            {
                output += args[x];
            }
            logger.WriteLine("staring client with:" + output,LogLevel.Info);

            bool hasArgs = args.Any();

            var argumentService = new ArgumentService(hasArgs, args, logger, ObjectFactory.Container.GetInstance<IQueueableTaskManager>());
            if (!hasArgs || argumentService.IsServer )
            {

                
#if DEBUG
                presenterFactory.InstantiatePresenter<MainWindow>().Show();
                if (hasArgs)
                {
                    argumentService.ProcessArguments(args);
                }
                Application.Run();

#else
            try
            {
                presenterFactory.InstantiatePresenter<MainWindow> ().Show ();
                Application.Run ();
            }
            catch (Exception e)
            {
                logger.WriteLine("Application exited in an unusual way.", LogLevel.Fatal);
                logger.WriteException(e);
                crashReport.Report("global", e);

                Gtk.MessageDialog err = new MessageDialog (null, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, "The application has crashed in an unusual way.\n\nThe error has been logged to:\n"+ logger.LoggingPath);
                err.Run();

                Environment.Exit(0);


            }
#endif
           
               
            }
        }



    }
}
