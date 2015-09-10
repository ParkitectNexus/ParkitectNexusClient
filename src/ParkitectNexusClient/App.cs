// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Windows.Forms;
using CommandLine;
using ParkitectNexusClient.Properties;

namespace ParkitectNexusClient
{
    internal class App
    {
        private Parkitect Parkitect { get; } = new Parkitect();
        private ParkitectNexus ParkitectNexus { get; } = new ParkitectNexus();
        private CommandLineOptions Options { get; } = new CommandLineOptions();

        private bool EnsureParkitectInstalled()
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
                            "Parkitect Nexus Client", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
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
                                "Parkitect Nexus Client", MessageBoxButtons.YesNo, MessageBoxIcon.Error) != DialogResult.Yes)
                            return false;
                    }
                }
            }

            return true;
        }

        private void Download(string url)
        {
            // Try to parse the specified download url. If parsing fails open parkitect nexus. 
            ParkitectNexusUrl parkitectNexusUrl;
            if (!ParkitectNexusUrl.TryParse(url, out parkitectNexusUrl))
            {
                // Create a form to allow the dialogs to have a owner with forced focus and an icon.
                using (var focus = new FocusForm())
                {
                    focus.Show();
                    MessageBox.Show(focus, "The URL you opened is invalid!", "Parkitect Nexus Client", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                return;
            }

            // Run the download process in an installer form, for a nice visible process.
            Application.Run(new InstallerForm(Parkitect, ParkitectNexus, parkitectNexusUrl));
        }

        public void Run(string[] args)
        {
            // Try to parse the command-line arguments.
            Parser.Default.ParseArguments(args, Options);

            // Check for updates.
            var updateInfo = UpdateUtil.FindUpdate();
            if (updateInfo != null)
            {
                // Store set download url so it can be downloaded after the update.
                Settings.Default.DownloadOnNextRun = Options.DownloadUrl;
                Settings.Default.Save();

                using (var focus = new FocusForm())
                {
                    focus.Show();
                    if (
                        MessageBox.Show(focus,
                            "A required update for the Parkitect Nexus Client needs to be installed.\n" +
                            "Without this update you wont be able to install blueprints and savegames trough this application. The update should take less than a minute.\n" +
                            $"Would you like to install it now?\n\nYou are currently running v{typeof (ParkitectNexus).Assembly.GetName().Version}. The newest version is v{updateInfo.Version}",
                            "Parkitect Nexus Client", MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                        DialogResult.Yes)
                        return;


                    if (!UpdateUtil.InstallUpdate(updateInfo))
                        MessageBox.Show(focus, "Failed updating the update! Please try again later.", "Parkitect Nexus Client",
                            MessageBoxButtons.OK);

                    return;
                }
            }

            // Make sure the protocol has been installed.
            ParkitectNexus.InstallProtocol();

            // If no downloads are awaiting download, open nexus.
            if (Options.DownloadUrl == null && string.IsNullOrWhiteSpace(Settings.Default.DownloadOnNextRun))
            {
                if (!Options.Silent)
                    ParkitectNexus.Launch();
                return;
            }

            if (!EnsureParkitectInstalled())
                return;

            // Download assets.
            if (!string.IsNullOrWhiteSpace(Settings.Default.DownloadOnNextRun))
            {
                Download(Settings.Default.DownloadOnNextRun);
                Settings.Default.DownloadOnNextRun = null;
                Settings.Default.Save();
            }
            if (Options.DownloadUrl != null)
                Download(Options.DownloadUrl);
        }
    }
}