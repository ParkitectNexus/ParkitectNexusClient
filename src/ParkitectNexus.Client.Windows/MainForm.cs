// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using MetroFramework.Forms;
using ParkitectNexus.Client.Windows.SliderPanels;
using ParkitectNexus.Client.Windows.TabPages;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Game.Windows;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data;

namespace ParkitectNexus.Client.Windows
{
    public partial class MainForm : MetroForm
    {
        private SliderPanel _currentPanel;

        public MainForm()
        {
            //configure map
            StructureMap.Registry registry = ObjectFactory.ConfigureStructureMap();
            ObjectFactory.SetUpContainer(registry);

  
            IParkitect parkitect = ObjectFactory.Container.GetInstance<IParkitect>();
            IParkitectNexusWebsite website = new ParkitectNexusWebsite();
            IParkitectOnlineAssetRepository assetRepository = ObjectFactory.Container.GetInstance<IParkitectOnlineAssetRepository>();

            InitializeComponent();

            metroTabControl.TabPages.Add(new MenuTabPage(parkitect, website));
            metroTabControl.TabPages.Add(new ModsTabPage(parkitect));
            metroTabControl.TabPages.Add(new BlueprintsTabPage(parkitect));
            metroTabControl.TabPages.Add(new SavegamesTabPage(parkitect));
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