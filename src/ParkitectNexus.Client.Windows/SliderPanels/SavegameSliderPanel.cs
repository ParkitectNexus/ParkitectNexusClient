// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using ParkitectNexus.AssetMagic;

namespace ParkitectNexus.Client.Windows.SliderPanels
{
    public partial class SavegameSliderPanel : SliderPanel
    {
        public SavegameSliderPanel(string path, ISavegame savegame)
        {
            if (savegame == null) throw new ArgumentNullException(nameof(savegame));
            InitializeComponent();

            nameLabel.Text = savegame.Header.Name;
            pictureBox.Image = savegame.Screenshot;
        }
    }
}