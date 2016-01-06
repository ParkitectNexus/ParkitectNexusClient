using System;
using System.Drawing;
using ParkitectNexus.AssetMagic;

namespace ParkitectNexus.Client.Windows.SliderPanels
{
    public partial class BlueprintSliderPanel : SliderPanel
    {
        public BlueprintSliderPanel(string path, IBlueprint blueprint)
        {
            if (blueprint == null) throw new ArgumentNullException(nameof(blueprint));
            InitializeComponent();

            nameLabel.Text = blueprint.Header.Name;
            pictureBox.Image = Image.FromFile(path); // TODO: Check for exceptions (file deleted?)
        }
    }
}