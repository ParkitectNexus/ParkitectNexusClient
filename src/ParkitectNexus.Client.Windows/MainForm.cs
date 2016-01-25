﻿// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommandLine;
using MetroFramework;
using MetroFramework.Forms;
using ParkitectNexus.Client.Windows.Controls.SliderPanels;
using ParkitectNexus.Client.Windows.Controls.TabPages;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Utilities;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data.Web.Models;

namespace ParkitectNexus.Client.Windows
{
    public partial class MainForm : MetroForm, IPresenter
    {
        private readonly IParkitectNexusAuthManager _authManager;
        private SliderPanel _currentPanel;

        public MainForm(IPresenterFactory presenterFactory, ILogger logger,
            IParkitectNexusAuthManager authManager)
        {
            if (presenterFactory == null) throw new ArgumentNullException(nameof(presenterFactory));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (authManager == null) throw new ArgumentNullException(nameof(authManager));

            _authManager = authManager;

            logger.Open(Path.Combine(AppData.Path, "ParkitectNexusLauncher.log"));

            ParkitectNexusProtocol.Install(logger);

            TopLevel = true;
            InitializeComponent();

            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<MenuTabPage>());
            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<ModsTabPage>());
            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<BlueprintsTabPage>());
            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<SavegamesTabPage>());
            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<TasksTabPage>());

#if DEBUG
            Text += " (DEVELOPMENT BUILD)";
            developmentLabel.Enabled = true;
#endif

            if (_authManager.IsAuthenticated)
            {
                // TODO show actual user information, such as name and avatar
                FetchUserInfo();
            }
            else
            {
                SetUserName("Log in");
            }
        }

        private async void FetchUserInfo()
        {
            try
            {
                SetUserName("Loading...");

                var user = await _authManager.GetUser();

                SetUserName(user.Name);
                using (var av = await user.GetAvatar())
                {
                    if (av != null)
                        authLink.NoFocusImage = authLink.Image = ImageUtility.ResizeImage(av, 32, 32);
                }
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

        public void ProcessArguments(string[] args)
        {
            var options = new AppCommandLineOptions();
            Parser.Default.ParseArguments(args, options);

            if (options.Url != null)
            {
                ParkitectNexusUrl url;
                if (ParkitectNexusUrl.TryParse(options.Url, out url))
                {
                    switch (url.Action)
                    {
                        case ParkitectNexusUrlAction.Auth:
                            ProcessAuthUrl(url.Data as ParkitectNexusAuthUrlAction);
                            break;
                        case ParkitectNexusUrlAction.Install:
                            ProcessInstallUrl(url.Data as ParkitectNexusInstallUrlAction);
                            break;
                    }
                }
            }
        }

        private void ProcessAuthUrl(ParkitectNexusAuthUrlAction data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            _authManager.Key = data.Key;
            FetchUserInfo();
        }
        private void ProcessInstallUrl(ParkitectNexusInstallUrlAction data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
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

        private void metroTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tab = metroTabControl.SelectedTab as LoadableTilesTabPage;
            tab?.WasSelected();
        }

        #region Overrides of MetroForm

        protected override void WndProc(ref Message m)
        {
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

        private void ShowMe()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }

            // Force to top
            NativeMethods.SetForegroundWindow(Handle);
        }

        private void authLink_Click(object sender, EventArgs e)
        {
            if (!_authManager.IsAuthenticated)
            {
                _authManager.OpenLoginPage();
            }
            else
            {
                // TODO refresh subscriptions instead of logging out
                // _authManager.Logout();
                // SetUserName("Log in");
            }
        }
    }
}
