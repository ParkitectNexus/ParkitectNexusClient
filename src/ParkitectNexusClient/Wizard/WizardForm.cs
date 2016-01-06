// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ParkitectNexus.Client.Wizard
{
    /// <summary>
    ///     Represents an installation form.
    /// </summary>
    internal partial class WizardForm : Form
    {
        public WizardForm()
        {
            InitializeComponent();

            // Set the client size to make the baner fit snugly.
            ClientSize = new Size(493, 360);
        }

        /// <summary>
        ///     Attaches the specified wizard user control to the wizard form.
        /// </summary>
        /// <param name="wizardUserControl">The wizard user control.</param>
        public void Attach(BaseWizardUserControl wizardUserControl)
        {
            if (wizardUserControl == null) throw new ArgumentNullException(nameof(wizardUserControl));

            var u = userControlPanel.Controls.Count == 0 ? null : userControlPanel.Controls[0] as BaseWizardUserControl;
            u?.Detach();

            userControlPanel.Controls.Clear();
            userControlPanel.Controls.Add(wizardUserControl);
            wizardUserControl.Attach(this);
        }

        private void donateLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show(this, "Maintaining this client and adding new features takes a lot of time.\n" +
                                      "If you appreciate our work, please consider sending a donation our way!\n" +
                                      "All donations will be used for further development of the ParkitectNexus Client and the website.\n" +
                                      "\nSelect Yes to visit PayPal and send a donation.", "ParkitectNexus Client",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Process.Start("https://paypal.me/ikkentim");
            }
        }
    }
}