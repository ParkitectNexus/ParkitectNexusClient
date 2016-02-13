// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using ParkitectNexus.Data.Assets;

namespace ParkitectNexus.Client.Windows.Controls.SliderPanels
{
    public partial class SavegameSliderPanel : SliderPanel
    {
        public SavegameSliderPanel(IAsset savegame)
        {
            if (savegame == null) throw new ArgumentNullException(nameof(savegame));
            InitializeComponent();

            nameLabel.Text = savegame.Name;
            pictureBox.Image = savegame.GetImage();
        }
    }
}