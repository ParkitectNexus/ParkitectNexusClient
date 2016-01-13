// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using ParkitectNexus.Data.Game;
using ParkitectNexus.Data.Presenter;

namespace ParkitectNexus.Client.Windows.SliderPanels
{
    public partial class SavegameSliderPanel : SliderPanel, IPresenter
    {
        public SavegameSliderPanel(IParkitectAsset savegame)
        {
            if (savegame == null) throw new ArgumentNullException(nameof(savegame));
            InitializeComponent();

            nameLabel.Text = savegame.Name;
            pictureBox.Image = savegame.Thumbnail;
        }
    }
}
