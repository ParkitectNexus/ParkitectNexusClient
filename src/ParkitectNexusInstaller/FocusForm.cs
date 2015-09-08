// ParkitectNexusInstaller
// Copyright 2015 Parkitect, Tim Potze

using System.Drawing;
using System.Windows.Forms;

namespace ParkitectNexusInstaller
{
    internal partial class FocusForm : Form
    {
        public FocusForm()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.Manual;
            var rect = SystemInformation.VirtualScreen;
            Location = new Point(rect.Bottom + 10, rect.Right + 10);
        }

        public new void Show()
        {
            base.Show();
            Focus();
            BringToFront();
        }
    }
}