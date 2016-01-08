using CommandLine;
using ParkitectNexus.Client.Settings;
using ParkitectNexus.Client.Wizard;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Reporting;
using ParkitectNexus.Data.Settings;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkitectNexus.Client
{
    public class Client : IClient
    {
        public Client(string[] args, ICrashReporterFactory reportingFactory, IParkitect parkitect,IParkitectOnlineAssetRepository parkitectOnlineAssetRepository, IParkitectNexusWebsite parkitectNexusWebsite, IOperatingSystem operatingSystem,IRepositoryFactory repositoryFactory,IPathResolver pathResolver)
        {
            var options = new CommandLineOptions();
            var settings = repositoryFactory.Repository<ClientSettings>();
            Parser.Default.ParseArguments(args, options);

            Log.Open(Path.Combine(pathResolver.AppData(), "ParkitectNexusLauncher.log"));
            Log.MinimumLogLevel = options.LogLevel;

            try
            {
                Log.WriteLine($"Application was launched with arguments '{string.Join(" ", args)}'.", LogLevel.Info);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Check for updates. If updates are available, do not resume usual logic.
                if (CheckForUpdates(parkitectNexusWebsite, options)) return;

                if (operatingSystem.GetOperatingSystem() == SupportedOperatingSystem.Windows)
                    ParkitectNexusProtocol.Install();

                // Ensure parkitect has been installed. If it has not been installed, quit the application.
                if (!EnsureParkitectInstalled(parkitect, options))
                    return;

                if (operatingSystem.GetOperatingSystem() == SupportedOperatingSystem.Windows)
                    UpdateUtil.MigrateMods(parkitect);

                ModLoaderUtil.InstallModLoader(parkitect);


                // Install backlog.
                if (!string.IsNullOrWhiteSpace(settings.Model.DownloadOnNextRun))
                {
                    Download(settings.Model.DownloadOnNextRun, parkitect, parkitectOnlineAssetRepository);
                    settings.Model.DownloadOnNextRun = null;
                    settings.Save();
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
                if (options.Silent && !settings.Model.BootOnNextRun)
                    return;

                settings.Model.BootOnNextRun = false;
                settings.Save();

                var form = new WizardForm();
                form.Attach(new MenuUserControl(parkitect, parkitectNexusWebsite, parkitectOnlineAssetRepository, reportingFactory));
                Application.Run(form);
            }
            catch (Exception e)
            {
                Log.WriteLine("Application exited in an unusual way.", LogLevel.Fatal);
                Log.WriteException(e);
                reportingFactory.Report("global", e);

                using (var focus = new FocusForm())
                {
                    MessageBox.Show(focus,
                        $"The application has crashed in an unusual way.\n\nThe error has been logged to:\n{Log.LoggingPath}",
                        "ParkitectNexus Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            Log.Close();
        }
        private static bool EnsureParkitectInstalled(IParkitect parkitect, CommandLineOptions options)
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

        private static void Download(string url, IParkitect parkitect,
            IParkitectOnlineAssetRepository parkitectOnlineAssetRepository)
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

        private static bool CheckForUpdates(IParkitectNexusWebsite website, CommandLineOptions options)
        {
#if DEBUG
            return false;
#else
            var settings = new ClientSettings();
            var updateInfo = UpdateUtil.CheckForUpdates(website);
            if (updateInfo != null)
            {
                // Store download url so it can be downloaded after the update.
                if (!string.IsNullOrEmpty(options.DownloadUrl))
                    settings.DownloadOnNextRun = options.DownloadUrl;
                else
                    settings.BootOnNextRun = !options.Silent;

                settings.Save();

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
