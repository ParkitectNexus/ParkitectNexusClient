// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
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
    }
}