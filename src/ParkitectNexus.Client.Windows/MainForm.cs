// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.IO;
using MetroFramework.Forms;
using ParkitectNexus.Client.Windows.SliderPanels;
using ParkitectNexus.Client.Windows.TabPages;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Presenter;
using ParkitectNexus.Data.Utilities;

namespace ParkitectNexus.Client.Windows
{
    public partial class MainForm : MetroForm, IPresenter
    {
        private readonly IPresenterFactory _presenterFactory;
        private SliderPanel _currentPanel;

        public MainForm(IPresenterFactory presenterFactory, ILogger logger)
        {
            if (presenterFactory == null) throw new ArgumentNullException(nameof(presenterFactory));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _presenterFactory = presenterFactory;

            logger.Open(Path.Combine(AppData.Path, "ParkitectNexusLauncher.log"));

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
    }
}
