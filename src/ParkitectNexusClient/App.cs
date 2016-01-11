// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using System.Windows.Forms;
using CommandLine;
using ParkitectNexus.Client.Settings;
using ParkitectNexus.Client.Wizard;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Reporting;
using ParkitectNexus.Data.Settings;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.Client;

namespace ParkitectNexus.Client
{
    public class App : IApp
    {
        private readonly string[] _args;
        private readonly ICrashReporterFactory _reportingFactory;
        private readonly IParkitect _parkitect;
        private readonly IParkitectOnlineAssetRepository _parkitectOnlineAssetRepository;
        private readonly IParkitectNexusWebsite _parkitectNexusWebsite;
        private readonly IOperatingSystem _operatingSystem;
        private readonly ISettingsRepositoryFactory _settingsRepositoryFactory;
        private readonly ILogger _logger;

        private readonly CommandLineOptions _options = new CommandLineOptions();

        public App(string[] args, ICrashReporterFactory reportingFactory, IParkitect parkitect,
            IParkitectOnlineAssetRepository parkitectOnlineAssetRepository, IParkitectNexusWebsite parkitectNexusWebsite,
            IOperatingSystem operatingSystem, ISettingsRepositoryFactory settingsRepositoryFactory, ILogger logger)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            if (parkitectOnlineAssetRepository == null)
                throw new ArgumentNullException(nameof(parkitectOnlineAssetRepository));
            if (parkitectNexusWebsite == null) throw new ArgumentNullException(nameof(parkitectNexusWebsite));
            if (operatingSystem == null) throw new ArgumentNullException(nameof(operatingSystem));
            if (settingsRepositoryFactory == null) throw new ArgumentNullException(nameof(settingsRepositoryFactory));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            _args = args;
            _reportingFactory = reportingFactory;
            _parkitect = parkitect;
            _parkitectOnlineAssetRepository = parkitectOnlineAssetRepository;
            _parkitectNexusWebsite = parkitectNexusWebsite;
            _operatingSystem = operatingSystem;
            _settingsRepositoryFactory = settingsRepositoryFactory;
            _logger = logger;

            Parser.Default.ParseArguments(args, _options);
        }

        public void Run()
        {
            var settings = _settingsRepositoryFactory.Repository<ClientSettings>();

            _logger.Open(Path.Combine(AppData.Path, "ParkitectNexusLauncher.log"));
            _logger.MinimumLogLevel = _options.LogLevel;

            try
            {
                _logger.WriteLine($"Application was launched with arguments '{string.Join(" ", _args)}'.", LogLevel.Info);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Check for updates. If updates are available, do not resume usual logic.
                if (CheckForUpdates(_parkitectNexusWebsite, _options)) return;

                if (_operatingSystem.Detect() == SupportedOperatingSystem.Windows)
                    ParkitectNexusProtocol.Install(_logger);

                // Ensure parkitect has been installed. If it has not been installed, quit the application.
                if (!EnsureParkitectInstalled(_parkitect, _options))
                    return;

                if (_operatingSystem.Detect() == SupportedOperatingSystem.Windows)
                    UpdateUtil.MigrateMods(_parkitect);

                ModLoaderUtil.InstallModLoader(_parkitect, _logger);


                // Install backlog.
                if (!string.IsNullOrWhiteSpace(settings.Model.DownloadOnNextRun))
                {
                    Download(settings.Model.DownloadOnNextRun);
                    settings.Model.DownloadOnNextRun = null;
                    settings.Save();
                }

                // Process download option.
                if (_options.DownloadUrl != null)
                {
                    Download(_options.DownloadUrl);
                    return;
                }

                // If the launch option has been used, launch the game.
                if (_options.Launch)
                {
                    _parkitect.Launch();
                    return;
                }

                // Handle silent calls.
                if (_options.Silent && !settings.Model.BootOnNextRun)
                    return;

                settings.Model.BootOnNextRun = false;
                settings.Save();

                var form = new WizardForm();
                form.Attach(new MenuUserControl(_parkitect, _parkitectNexusWebsite, _parkitectOnlineAssetRepository,
                    _reportingFactory, _logger));
                Application.Run(form);
            }
            catch (Exception e)
            {
                _logger.WriteLine("Application exited in an unusual way.", LogLevel.Fatal);
                _logger.WriteException(e);
                _reportingFactory.Report("global", e);

                using (var focus = new FocusForm())
                {
                    MessageBox.Show(focus,
                        $"The application has crashed in an unusual way.\n\nThe error has been logged to:\n{_logger.LoggingPath}",
                        "ParkitectNexus Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            _logger.Close();
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

        private void Download(string url)
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
            form.Attach(new InstallAssetUserControl(_parkitect, _parkitectOnlineAssetRepository, _logger,
                parkitectNexusUrl, null));
            Application.Run(form);
        }

        private bool CheckForUpdates(IParkitectNexusWebsite website, CommandLineOptions options)
        {
#if DEBUG
            return false;
#else
            var repositoryFactory = ObjectFactory.GetInstance<ISettingsRepositoryFactory>();
            var webFactory = ObjectFactory.GetInstance<IParkitectNexusWebFactory>();

            var settings = repositoryFactory.Repository<ClientSettings>();
            var updateInfo = UpdateUtil.CheckForUpdates(website, webFactory);

            if (updateInfo != null)
            {
                // Store download url so it can be downloaded after the update.
                if (!string.IsNullOrEmpty(options.DownloadUrl))
                    settings.Model.DownloadOnNextRun = options.DownloadUrl;
                else
                    settings.Model.BootOnNextRun = !options.Silent;

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

                    if (!UpdateUtil.InstallUpdate(updateInfo, webFactory))
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
