﻿// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using MetroFramework.Forms;
using ParkitectNexus.Client.Windows.SliderPanels;
using ParkitectNexus.Client.Windows.TabPages;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Game.Windows;
using ParkitectNexus.Data.Web;
using ParkitectNexus.Data;
using ParkitectNexus.Data.Presenter;

namespace ParkitectNexus.Client.Windows
{
    public partial class MainForm : MetroForm, IPresenter
    {
        private SliderPanel _currentPanel;

        public MainForm(IPresenterFactory presenterFactory)
        {
            InitializeComponent();

            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<MenuTabPage>() );
            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<ModsTabPage>());
            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<BlueprintsTabPage>());
            metroTabControl.TabPages.Add(presenterFactory.InstantiatePresenter<SavegamesTabPage>());
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