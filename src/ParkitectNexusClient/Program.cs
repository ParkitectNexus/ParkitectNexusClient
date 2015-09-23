// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Windows.Forms;

namespace ParkitectNexus.Client
{
    /// <summary>
    ///     Represents the entry point class for the application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            UpdateUtil.MigrateSettings();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var app = new App();
            app.Run(args);
        }
    }
}