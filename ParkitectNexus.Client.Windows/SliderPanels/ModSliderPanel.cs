﻿// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using ParkitectNexus.Data.Game;

namespace ParkitectNexus.Client.Windows.SliderPanels
{
    public partial class ModSliderPanel : SliderPanel
    {
        private readonly IParkitectMod _mod;

        public ModSliderPanel(IParkitectMod mod)
        {
            if (mod == null) throw new ArgumentNullException(nameof(mod));
            _mod = mod;

            InitializeComponent();

            nameLabel.Text = mod.Name;
            versionLabel.Text = mod.Tag;
            enableModToggle.Checked = _mod.IsEnabled;
        }

        private void enableModToggle_CheckedChanged(object sender, EventArgs e)
        {
            _mod.IsEnabled = enableModToggle.Checked;
        }
    }
}