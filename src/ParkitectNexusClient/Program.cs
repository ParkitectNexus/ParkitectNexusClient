// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Windows.Forms;
using CommandLine;
using ParkitectNexus.Client.Properties;
using ParkitectNexus.Client.Wizard;
using ParkitectNexus.Data;

namespace ParkitectNexus.Client
{
    /// <summary>
    ///     Represents the entry point class for the application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Gets the an instance of <see cref="Parkitect"/> which represents the game.
        /// </summary>
        private static Parkitect Parkitect { get; } = new Parkitect();

        /// <summary>
        /// Gets the an instance of <see cref="ParkitectNexusWebsite"/> which represents the game.
        /// </summary>
        private static ParkitectNexusWebsite ParkitectNexusWebsite { get; } = new ParkitectNexusWebsite();

        private static ParkitectOnlineAssetRepository ParkitectOnlineAssetRepository { get; } = new ParkitectOnlineAssetRepository(ParkitectNexusWebsite);

        /// <summary>
        /// Gets the command line options used to launch the client.
        /// </summary>
        private static CommandLineOptions Options { get; } = new CommandLineOptions();

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            UpdateUtil.MigrateSettings();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check for updates. If updates are available, do not resume usual logic.
            if (CheckForUpdates()) return;

            // Initialize.
            Parser.Default.ParseArguments(args, Options);
            ParkitectNexusWebsite.InstallProtocol();

            // Ensure parkitect has been installed. If it has not been installed, quit the application.
            if (!EnsureParkitectInstalled())
                return;

            // Install backlog.
            if (!string.IsNullOrWhiteSpace(Settings.Default.DownloadOnNextRun))
            {
                Download(Settings.Default.DownloadOnNextRun);
                Settings.Default.DownloadOnNextRun = null;
                Settings.Default.Save();
            }

            // Process download option.
            if (Options.DownloadUrl != null)
            {
                Download(Options.DownloadUrl);

                return;
            }

            // If the launch option has been used, launch the game.
            if (Options.Launch)
            {
                Parkitect.LaunchWithMods();
                return;
            }

            // Handle silent calls.
            if (Options.Silent && !Settings.Default.BootOnNextRun)
                return;

            Settings.Default.BootOnNextRun = false;
            Settings.Default.Save();

            var form = new WizardForm();
            form.Attach(new MenuUserControl(Parkitect, ParkitectNexusWebsite, ParkitectOnlineAssetRepository));
            Application.Run(form);
        }

        private static bool EnsureParkitectInstalled()
        {
            // Detect Parkitect. If it could not be found ask the user to locate it.
            Parkitect.DetectInstallationPath();

            // If an installation path is passed as argument, try and use it.
            if (Options.SetInstallationPath != null)
                Parkitect.SetInstallationPathIfValid(Options.SetInstallationPath);

            if (!Parkitect.IsInstalled)
            {
                // Create a form to allow the dialogs to have a owner with forced focus and an icon.
                using (var focus = new FocusForm())
                {
                    focus.Show();

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
                    while (!Parkitect.IsInstalled)
                    {
                        if (dialog.ShowDialog(focus) != DialogResult.OK) return false;

                        if (!Parkitect.SetInstallationPathIfValid(dialog.SelectedPath) &&
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

        private static void Download(string url)
        {
            // Try to parse the specified download url. If parsing fails open ParkitectNexus. 
            ParkitectNexusUrl parkitectNexusUrl;
            if (!ParkitectNexusUrl.TryParse(url, out parkitectNexusUrl))
            {
                // Create a form to allow the dialogs to have a owner with forced focus and an icon.
                using (var focus = new FocusForm())
                {
                    focus.Show();
                    MessageBox.Show(focus, "The URL you opened is invalid!", "ParkitectNexus Client",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                return;
            }

            // Run the download process in an installer form, for a nice visible process.
            var form = new WizardForm();
            form.Attach(new InstallAssetUserControl(Parkitect, ParkitectOnlineAssetRepository, parkitectNexusUrl, null));
            Application.Run(form);
        }

        private static bool CheckForUpdates()
        {
#if DEBUG
            return false;
#else
            var updateInfo = UpdateUtil.CheckForUpdates();
            if (updateInfo != null)
            {
                // Store download url so it can be downloaded after the update.
                if (!string.IsNullOrEmpty(Options.DownloadUrl))
                    Settings.Default.DownloadOnNextRun = Options.DownloadUrl;
                else
                    Settings.Default.BootOnNextRun = true;

                Settings.Default.Save();

                using (var focus = new FocusForm())
                {
                    focus.Show();
                    if (
                        MessageBox.Show(focus,
                            "A required update for the ParkitectNexus Client needs to be installed.\n" +
                            "Without this update you won't be able to install blueprints, savegames or mods trough this application. The update should take less than a minute.\n" +
                            $"Would you like to install it now?\n\nYou are currently running v{typeof (Program).Assembly.GetName().Version}. The newest version is v{updateInfo.Version}",
                            "ParkitectNexus Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                        DialogResult.Yes)
                        return true;


                    if (!UpdateUtil.InstallUpdate(updateInfo))
                        MessageBox.Show(focus, "Failed updating the update! Please try again later.", "ParkitectNexus Client",
                            MessageBoxButtons.OK);

                    return true;
                }
            }

            return false;
#endif
        }
    }
}