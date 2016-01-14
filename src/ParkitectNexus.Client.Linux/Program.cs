using System;
using Gtk;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Reporting;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client.Linux
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Application.Init ();

            //configure map
            StructureMap.Registry registry = ObjectFactory.ConfigureStructureMap();
            ObjectFactory.SetUpContainer(registry);

            var presenterFactory = ObjectFactory.Container.GetInstance<IPresenterFactory>();
            var crashReport = ObjectFactory.Container.GetInstance<ICrashReporterFactory> ();
            var logger = ObjectFactory.Container.GetInstance<ILogger> ();
#if DEBUG
            presenterFactory.InstantiatePresenter<MainWindow>().Show();
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
