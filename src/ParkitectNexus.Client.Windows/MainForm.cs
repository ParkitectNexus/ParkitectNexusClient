// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Forms;
using ParkitectNexus.Client.Windows.SliderPanels;
using ParkitectNexus.Client.Windows.TabPages;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client.Windows
{
    public partial class MainForm : MetroForm, IPresenter
    {
        private SliderPanel _currentPanel;

        public MainForm(IPresenterFactory presenterFactory, ILogger logger)
        {
            if (presenterFactory == null) throw new ArgumentNullException(nameof(presenterFactory));
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            logger.Open(Path.Combine(AppData.Path, "ParkitectNexusLauncher.log"));

            TopLevel = true;
            InitializeComponent();

            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<MenuTabPage>());
            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<ModsTabPage>());
            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<BlueprintsTabPage>());
            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<SavegamesTabPage>());

#if DEBUG
            Text += " (DEVELOPMENT BUILD)";
            developmentLabel.Enabled = true;
#endif
        }

        public void ProcessArguments(string[] args)
        {
            //
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
    }
}
