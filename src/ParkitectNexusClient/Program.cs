// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using CommandLine;
using ParkitectNexus.Client.Properties;
using ParkitectNexus.Client.Wizard;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Utilities;

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
            var parkitect = new Parkitect();
            var parkitectNexusWebsite = new ParkitectNexusWebsite();
            var parkitectOnlineAssetRepository = new ParkitectOnlineAssetRepository(parkitectNexusWebsite);
            var options = new CommandLineOptions();
            
            UpdateUtil.MigrateSettings();

            Parser.Default.ParseArguments(args, options);

            Log.Open(Path.Combine(AppData.Path, "ParkitectNexusLauncher.log"));
            Log.MinimumLogLevel = options.LogLevel;

            try
            {
                Log.WriteLine($"Application was launched with arguments '{string.Join(" ", args)}'.", LogLevel.Info);
                
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Check for updates. If updates are available, do not resume usual logic.
                if (CheckForUpdates(parkitectNexusWebsite, options)) return;
                
                parkitectNexusWebsite.InstallProtocol();

                // Ensure parkitect has been installed. If it has not been installed, quit the application.
                if (!EnsureParkitectInstalled(parkitect, options))
                    return;

                UpdateUtil.MigrateMods(parkitect);
                ModLoaderUtil.InstallModLoader(parkitect);

                // Install backlog.
                if (!string.IsNullOrWhiteSpace(Settings.Default.DownloadOnNextRun))
                {
                    Download(Settings.Default.DownloadOnNextRun, parkitect, parkitectOnlineAssetRepository);
                    Settings.Default.DownloadOnNextRun = null;
                    Settings.Default.Save();
                }

                // Process download option.
                if (options.DownloadUrl != null)
                {
                    Download(options.DownloadUrl, parkitect, parkitectOnlineAssetRepository);
                    return;
                }

                // If the launch option has been used, launch the game.
                if (options.Launch)
                {
                    parkitect.Launch();
                    return;
                }

                // Handle silent calls.
                if (options.Silent && !Settings.Default.BootOnNextRun)
                    return;

                Settings.Default.BootOnNextRun = false;
                Settings.Default.Save();

                var form = new WizardForm();
                form.Attach(new MenuUserControl(parkitect, parkitectNexusWebsite, parkitectOnlineAssetRepository));
                Application.Run(form);
            }
            catch (Exception e)
            {
                Log.WriteLine("Application exited in an unusual way.", LogLevel.Fatal);
                Log.WriteException(e);
                CrashReporter.Report("global", parkitect, parkitectNexusWebsite, e);

                using (var focus = new FocusForm())
                {
                    MessageBox.Show(focus,
                        $"Launching the game with mods failed.\n\nThe error has been logged to:\n{Log.LoggingPath}",
                        "ParkitectNexus Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            Log.Close();
        }

        private static bool EnsureParkitectInstalled(Parkitect parkitect, CommandLineOptions options)
        {
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
                    if (
                        MessageBox.Show(focus, "We couldn't detect Parkitect on your machine.\nPlease point me to it!",
                            "ParkitectNexus Client", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) !=
                        DialogResult.OK)
                        return false;

                    var dialog = new FolderBrowserDialog
                    {
                        ShowNewFolderButton = false,
                        Description = "Please show us where you have installed Parkitect.",
                        RootFolder = Environment.SpecialFolder.MyComputer
                    };

                    // Keep showing folder browser dialog until an installation directory has been picked or the user has canceled.
                    while (!parkitect.IsInstalled)
                    {
                        if (dialog.ShowDialog(focus) != DialogResult.OK) return false;

                        if (!parkitect.SetInstallationPathIfValid(dialog.SelectedPath) &&
                            MessageBox.Show(focus,
                                "The folder you selected does not contain Parkitect!\nWould you like to try again?",
                                "ParkitectNexus Client", MessageBoxButtons.YesNo, MessageBoxIcon.Error) !=
                            DialogResult.Yes)
                            return false;
                    }
                }
            }

            return true;
        }

        private static void Download(string url, Parkitect parkitect,
            ParkitectOnlineAssetRepository parkitectOnlineAssetRepository)
        {
            // Try to parse the specified download url. If parsing fails open ParkitectNexus. 
            ParkitectNexusUrl parkitectNexusUrl;
            if (!ParkitectNexusUrl.TryParse(url, out parkitectNexusUrl))
            {
                // Create a form to allow the dialogs to have a owner with forced focus and an icon.
                using (var focus = new FocusForm())
                {
                    MessageBox.Show(focus, "The URL you opened is invalid!", "ParkitectNexus Client",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                return;
            }

            // Run the download process in an installer form, for a nice visible process.
            var form = new WizardForm();
            form.Attach(new InstallAssetUserControl(parkitect, parkitectOnlineAssetRepository, parkitectNexusUrl, null));
            Application.Run(form);
        }

        private static bool CheckForUpdates(ParkitectNexusWebsite website, CommandLineOptions options)
        {
#if DEBUG
            return false;
#else

            var updateInfo = UpdateUtil.CheckForUpdates(website);
            if (updateInfo != null)
            {
                // Store download url so it can be downloaded after the update.
                if (!string.IsNullOrEmpty(options.DownloadUrl))
                    Settings.Default.DownloadOnNextRun = options.DownloadUrl;
                else
                    Settings.Default.BootOnNextRun = !options.Silent;

                Settings.Default.Save();

                using (var focus = new FocusForm())
                {
                    if (
                        MessageBox.Show(focus,
                            "A required update for the ParkitectNexus Client needs to be installed.\n" +
                            "Without this update you won't be able to install blueprints, savegames or mods trough this application. The update should take less than a minute.\n" +
                            $"Would you like to install it now?\n\nYou are currently running v{typeof (Program).Assembly.GetName().Version}. The newest version is v{updateInfo.Version}",
                            "ParkitectNexus Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                        DialogResult.Yes)
                        return true;


                    if (!UpdateUtil.InstallUpdate(updateInfo))
                        MessageBox.Show(focus, "Failed installing the update! Please try again later.",
                            "ParkitectNexus Client",
                            MessageBoxButtons.OK);

                    return true;
                }
            }

            return false;
#endif
        }
    }
}