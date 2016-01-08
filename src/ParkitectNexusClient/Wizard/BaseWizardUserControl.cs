// ParkitectNexusClient
// Copyright 2015 Parkitect, Tim Potze

using System;
using System.Drawing;
using System.Windows.Forms;

namespace ParkitectNexus.Client.Wizard
{
    internal partial class BaseWizardUserControl : UserControl
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseWizardUserControl" /> class.
        /// </summary>
        public BaseWizardUserControl()
        {
            InitializeComponent();
            Size = new Size(493, 302);
        }

        /// <summary>
        ///     Gets the wizard form this control is attached to.
        /// </summary>
        public WizardForm WizardForm { get; private set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to draw a footer line.
        /// </summary>
        protected bool DrawFooterLine { get; set; } = true;

        /// <summary>
        ///     Attaches this instance to the specified wizard form.
        /// </summary>
        /// <param name="wizardForm">The wizard form.</param>
        public void Attach(WizardForm wizardForm)
        {
            WizardForm = wizardForm;
            OnAttached();
        }

        /// <summary>
        ///     Detaches this instance from the wizard form it is attached to.
        /// </summary>
        public void Detach()
        {
            OnDetached();
            WizardForm = null;
        }

        public event EventHandler Attached;
        public event EventHandler Detached;

        /// <summary>
        ///     Raises the <see cref="E:Attached" /> event.
        /// </summary>
        protected virtual void OnAttached()
        {
            Attached?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Raises the <see cref="E:Detached" /> event.
        /// </summary>
        protected virtual void OnDetached()
        {
            Detached?.Invoke(this, EventArgs.Empty);
        }

        #region Overrides of Control

        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (DrawFooterLine)
            {
                e.Graphics.DrawLine(new Pen(Color.FromArgb(182, 182, 182)), 0, 312 - 58, 488, 312 - 58);
                e.Graphics.DrawLine(new Pen(Color.FromArgb(252, 252, 252)), 0, 313 - 58, 488, 313 - 58);
                e.Graphics.DrawLine(new Pen(Color.FromArgb(252, 252, 252)), 489, 312 - 58, 489, 313 - 58);
            }
        }

        #endregion
    }
}