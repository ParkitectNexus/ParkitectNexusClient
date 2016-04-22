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

namespace ParkitectNexus.Client.Linux
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
         
            // Look and see if this is the only running instance of the client.
            try
            {
                // Initialize the structure map container.
                var registry = ObjectFactory.ConfigureStructureMap();
                registry.IncludeRegistry(new PresenterRegistry());
                ObjectFactory.SetUpContainer(registry);


                // Create the form and run its message loop. If arguments were specified, process them within the
                // form.
                var presenterFactory = ObjectFactory.GetInstance<IPresenterFactory>();
                var app = presenterFactory.InstantiatePresenter<App>();
                if (!app.Initialize(ToolkitType.Gtk))
                    return;
                

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

       
    }
}
    