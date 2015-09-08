// ParkitectNexusInstaller
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Windows.Forms;
using CommandLine;

// ReSharper disable ExpressionIsAlwaysNull

namespace ParkitectNexusInstaller
{
    /// <summary>
    /// Represents the entry point class for the application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            // Check for updates.
            ClickOnceUpdater.Update();
            ClickOnceUpdater.UpdateSettings();

            var parkitect = new Parkitect();
            var parkitectNexus = new ParkitectNexus();
            var options = new CommandLineOptions();

            // Enable application visual styles.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Make sure the protocol has been installed.
            parkitectNexus.InstallProtocol();

            // Try to parse the command-line arguments. If parsing fails open parkitect nexus.
            if (!Parser.Default.ParseArguments(args, options))
            {
                parkitectNexus.Launch();
                return;
            }

            // Detect Parkitect. If it could not be found ask the user to locate it.
            parkitect.DetectInstallationPath();

            // If an installation path is passed as argument, try and use it.
            if (options.SetInstallationPath != null)
                parkitect.SetInstallationPathIfValid(options.SetInstallationPath);

            if (!parkitect.IsInstalled)
            {
                // Create a form to allow the dialogs to have a owner with forced focus and an icon.
                using (var focus = new FocusForm())
                {
                    focus.Show();

                    if (
                        MessageBox.Show(focus, "We couldn't detect Parkitect on your machine.\nPlease point me to it!",
                            "Parkitect Nexus", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                        return;

                    var dialog = new FolderBrowserDialog
                    {
                        ShowNewFolderButton = false,
                        Description = "Please show us where you have installed Parkitect.",
                        RootFolder = Environment.SpecialFolder.MyComputer
                    };

                    // Keep showing folder browser dialog until an installation directory has been picked or the user has canceled.
                    while (!parkitect.IsInstalled)
                    {
                        if (dialog.ShowDialog(focus) != DialogResult.OK) return;

                        if (!parkitect.SetInstallationPathIfValid(dialog.SelectedPath) &&
                            MessageBox.Show(focus,
                                "The folder you selected does not contain Parkitect!\nWould you like to try again?",
                                "Parkitect Nexus", MessageBoxButtons.YesNo, MessageBoxIcon.Error) != DialogResult.Yes)
                            return;
                    }
                }
            }

            // If no download option was specified, launch parkitect nexus.
            if (options.DownloadUrl == null)
            {
                parkitectNexus.Launch();
                return;
            }

            // Try to parse the specified download url. If parsing fails open parkitect nexus. 
            ParkitectNexusUrl parkitectNexusUrl;
            if (!ParkitectNexusUrl.TryParse(options.DownloadUrl, out parkitectNexusUrl))
            {

                // Create a form to allow the dialogs to have a owner with forced focus and an icon.
                using (var focus = new FocusForm())
                {
                    focus.Show();
                    MessageBox.Show(focus, "The URL you opened is invalid!", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                return;
            }

            // Run the download process in an installer form, for a nice visible process.
            Application.Run(new InstallerForm(parkitect, parkitectNexus, parkitectNexusUrl));
        }
    }
}