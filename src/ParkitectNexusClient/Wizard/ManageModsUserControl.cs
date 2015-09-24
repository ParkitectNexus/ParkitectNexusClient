using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParkitectNexus.Data;

namespace ParkitectNexus.Client.Wizard
{
    internal partial class ManageModsUserControl : BaseWizardUserControl
    {
        private readonly Parkitect _parkitect;

        public ManageModsUserControl(Parkitect parkitect)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));

            _parkitect = parkitect;
            InitializeComponent();
        }
    }
}
