// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Drawing;
using ParkitectNexus.Data.Assets;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;

namespace ParkitectNexus.Client.Windows.SliderPanels
{
    public partial class BlueprintSliderPanel : SliderPanel, IPresenter
    {
        public BlueprintSliderPanel(IAsset blueprint)
        {
            if (blueprint == null) throw new ArgumentNullException(nameof(blueprint));
            InitializeComponent();

            nameLabel.Text = blueprint.Name;
            pictureBox.Image = Image.FromStream(blueprint.Open()); // TODO: Check for exceptions (file deleted?)
        }
    }
}
