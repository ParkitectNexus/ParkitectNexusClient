using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            pictureBox.Image = Image.FromFile(path);// TODO: Check for exceptions (file deleted?)

        }
    }
}
