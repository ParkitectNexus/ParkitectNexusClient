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
