// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CommandLine;
using MetroFramework;
using MetroFramework.Forms;
using ParkitectNexus.Client.Windows.Controls.SliderPanels;
using ParkitectNexus.Client.Windows.Controls.TabPages;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Assets.Modding;
using ParkitectNexus.Data.Authentication;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Tasks;
using ParkitectNexus.Data.Tasks.Prefab;
using ParkitectNexus.Data.Updating;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.Models;

namespace ParkitectNexus.Client.Windows
{
    public partial class MainForm : MetroForm, IPresenter
    {
        private readonly ILogger _log;
        private readonly IAuthManager _authManager;
        private readonly IQueueableTaskManager _taskManager;
        private readonly IParkitect _parkitect;
        private readonly IUpdateManager _updateManager;
        private SliderPanel _currentPanel;

        public MainForm(IPresenterFactory presenterFactory, ILogger log,
            IAuthManager authManager, IQueueableTaskManager taskManager, IParkitect parkitect, IUpdateManager updateManager)
        {
            _log = log;
            _authManager = authManager;
            _taskManager = taskManager;
            _parkitect = parkitect;
            _updateManager = updateManager;

            // Hook onto the authentication manager events.
            _authManager.Authenticated += (sender, args) => FetchUserInfo();

            // Open the logger and install the modloader and parkitect nexus protocol.
            log.Open(Path.Combine(AppData.Path, "ParkitectNexusLauncher.log"));
            log.MinimumLogLevel = LogLevel.Debug;

            ModLoaderUtil.InstallModLoader(_parkitect, log);
            ParkitectNexusProtocol.Install(log);

            // Initialize the compontents in this form.
            InitializeComponent();
            TopLevel = true;

            // Add tab pages to the tab control.
            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<MenuTabPage>());
            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<ModsTabPage>());
            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<BlueprintsTabPage>());
            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<SavegamesTabPage>());
            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<TasksTabPage>());

            // Display 'development build' message if debug mode is enabled.
#if DEBUG
            Text += " (DEVELOPMENT BUILD)";
            developmentLabel.Visible = true;
#endif

            // Set user box image and name.
            if (_authManager.IsAuthenticated)
                FetchUserInfo();
            else
                SetUserName("Log in");

            // Fetch updates
            // TODO: Disable update checking; It takes too many GH calls. Reimplementing when PN reports version numbers.
//            if (assetUpdatesManager.ShouldCheckForUpdates())
//                _taskManager.Add<CheckForUpdatesTask>();
        }

        public bool ProcessArguments(string[] args)
        {
            _log.WriteLine($"Received arguments: '{string.Join(" ", args)}", LogLevel.Info);

            var options = new AppCommandLineOptions();
            Parser.Default.ParseArguments(args, options);

            if (options.Url != null)
            {
                NexusUrl url;
                if (NexusUrl.TryParse(options.Url, out url))
                {
                    var attribute = url.Data.GetType().GetCustomAttribute<UrlActionTaskAttribute>();
                    if (attribute?.TaskType != null && typeof (UrlQueueableTask).IsAssignableFrom(attribute.TaskType))
                    {
                        var task = ObjectFactory.GetInstance<UrlQueueableTask>(attribute.TaskType);
                        task.Data = url.Data;

                        _taskManager.Add(task);

                        return true;
                    }
                }
            }

            if (options.Launch)
            {
                foreach (
                    var mod in
                        _parkitect.Assets[AssetType.Mod].OfType<IModAsset>().Where(m => m.Information.IsDevelopment))
                    _taskManager.With(mod).Add<CompileModTask>();


                _taskManager.Add<LaunchGameTask>();
                _taskManager.Add(new CloseAppTask(this));
                return false;
            }
            return true;
        }

        public void SpawnSliderPanel(SliderPanel panel)
        {
            if (_currentPanel != null)
            {
                Controls.Remove(_currentPanel);
                _currentPanel.Dispose();
                _currentPanel = null;
            }

            if (panel == null)
                return;

            _currentPanel = panel;

            Controls.Add(panel);
            Controls.SetChildIndex(panel, 0);
            panel.IsSlidedIn = true;
        }

        private async void FetchUserInfo()
        {
            try
            {
                SetUserName("Loading...");

                var user = await _authManager.GetUser();
                SetUserName(user.Name);

                var avatar = await _authManager.GetAvatar();
                if (avatar != null)
                    authLink.NoFocusImage = authLink.Image = avatar;
            }
            catch (Exception e)
            {
                // todo handle exceptions properly
            }
        }

        private void SetUserName(string name)
        {
            int width;
            using (var b = new Bitmap(1, 1))
            using (var g = Graphics.FromImage(b))
            using (var f = MetroFonts.Link(authLink.FontSize, authLink.FontWeight))
                width = (int)g.MeasureString(name, f).Width;

            authLink.Width = 32 + 8 + width;
            authLink.Text = name;
            authLink.Left = Width - authLink.Width - 7;
        }

        private void ShowMe()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }

            // Force to top
            NativeMethods.SetForegroundWindow(Handle);
        }

        private void metroTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tab = metroTabControl.SelectedTab as LoadableTilesTabPage;
            tab?.WasSelected();
        }

        private void authLink_Click(object sender, EventArgs e)
        {
            if (!_authManager.IsAuthenticated)
                _authManager.OpenLoginPage();
        }

        #region Overrides of Form

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Form.Shown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.EventArgs"/> that contains the event data. </param>
        protected override void OnShown(EventArgs e)
        {
            var update = _updateManager.CheckForUpdates<MainForm>();

            if (update != null)
            {
                if (
                    MetroMessageBox.Show(this,
                        "A required update for the ParkitectNexus Client needs to be installed.\n" +
                        "Without this update you won't be able to install blueprints, savegames or mods trough this application. The update should take less than a minute.\n" +
                        $"Would you like to install it now?\n\nYou are currently running v{typeof (Program).Assembly.GetName().Version}. The newest version is v{update.Version}",
                        "ParkitectNexus Client Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) !=
                    DialogResult.Yes)
                {
                    Close();
                    return;
                }

                if (!_updateManager.InstallUpdate(update))
                    MessageBox.Show(this, "Failed installing the update! Please try again later.",
                        "ParkitectNexus Client Update",
                        MessageBoxButtons.OK);

                Close();
            }

            // Ensure Parkitect has been installed.
            if (!_parkitect.IsInstalled)
            {
                if (
                    MetroMessageBox.Show(this, "We couldn't detect Parkitect on your machine.\nPlease point me to it!",
                        "Parkitect Installation", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) ==
                    DialogResult.Cancel)
                {
                    Close();
                    return;
                }

                var dialog = new FolderBrowserDialog
                {
                    Description = "Select your Parkitect installation folder.",
                    ShowNewFolderButton = false
                };

                while (!_parkitect.IsInstalled)
                {
                    if (dialog.ShowDialog(this) == DialogResult.Cancel)
                    {
                        Close();
                        return;
                    }

                    _parkitect.SetInstallationPathIfValid(dialog.SelectedPath);
                }
            }

            base.OnShown(e);
        }

        #endregion

        #region Overrides of MetroForm

        protected override void WndProc(ref Message m)
        {
            // Interprocess communication system. If the custom WM_GIVEFOCUS message was received,
            // read the ipc.dat file in the app data and pass it's arguments into the ProcessArguments method.
            if (m.Msg == NativeMethods.WM_GIVEFOCUS)
            {
                try
                {
                    var ipcPath = Path.Combine(AppData.Path, "ipc.dat");
                    if ((int) m.WParam == 1 && File.Exists(ipcPath))
                    {
                        ProcessArguments(File.ReadAllLines(ipcPath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray());

                        File.Delete(ipcPath);
                    }
                }
                catch (IOException)
                {
                }

                ShowMe();
            }
            base.WndProc(ref m);
        }

        #endregion

    }
}
