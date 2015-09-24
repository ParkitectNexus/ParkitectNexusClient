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
    internal partial class MenuUserControl : BaseWizardUserControl
    {
        private readonly Parkitect _parkitect;

        public MenuUserControl(Parkitect parkitect)
        {
            if (parkitect == null) throw new ArgumentNullException(nameof(parkitect));
            _parkitect = parkitect;
            InitializeComponent();
        }

        private void manageModsButton_Click(object sender, EventArgs e)
        {
            WizardForm.Attach(new ManageModsUserControl(_parkitect));
        }
    }
}
