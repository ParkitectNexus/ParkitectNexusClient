// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Drawing;
using System.Windows.Forms;

namespace ParkitectNexus.Client
{
    /// <summary>
    ///     Represents an invisible form used for acquiring focus.
    /// </summary>
    internal partial class FocusForm : Form
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FocusForm" /> class.
        /// </summary>
        public FocusForm()
        {
            InitializeComponent();

            // Spawn the form outside of the screen.
            Location = new Point(SystemInformation.VirtualScreen.Bottom + 10, SystemInformation.VirtualScreen.Right + 10);

            Show();
        }

        /// <summary>
        ///     Shows this instance.
        /// </summary>
        public new void Show()
        {
            base.Show();
            Focus();
            BringToFront();
        }
    }
}