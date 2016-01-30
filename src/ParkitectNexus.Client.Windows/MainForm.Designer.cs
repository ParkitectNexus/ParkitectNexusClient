using ParkitectNexus.Client.Windows.Controls.SliderPanels;

namespace ParkitectNexus.Client.Windows
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.metroTabControl = new MetroFramework.Controls.MetroTabControl();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroToggle1 = new MetroFramework.Controls.MetroToggle();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.developmentLabel = new MetroFramework.Controls.MetroLabel();
            this.authLink = new MetroFramework.Controls.MetroLink();
            this.SuspendLayout();
            // 
            // metroTabControl
            // 
            this.metroTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTabControl.Location = new System.Drawing.Point(20, 70);
            this.metroTabControl.Name = "metroTabControl";
            this.metroTabControl.Size = new System.Drawing.Size(682, 360);
            this.metroTabControl.TabIndex = 0;
            this.metroTabControl.UseSelectable = true;
            this.metroTabControl.SelectedIndexChanged += new System.EventHandler(this.metroTabControl_SelectedIndexChanged);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(23, 85);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(53, 19);
            this.metroLabel2.TabIndex = 4;
            this.metroLabel2.Text = "Enabed";
            // 
            // metroToggle1
            // 
            this.metroToggle1.AutoSize = true;
            this.metroToggle1.Location = new System.Drawing.Point(132, 86);
            this.metroToggle1.Name = "metroToggle1";
            this.metroToggle1.Size = new System.Drawing.Size(80, 17);
            this.metroToggle1.TabIndex = 3;
            this.metroToggle1.Text = "Off";
            this.metroToggle1.UseSelectable = true;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel1.Location = new System.Drawing.Point(23, 55);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(113, 25);
            this.metroLabel1.TabIndex = 2;
            this.metroLabel1.Text = "Coaster Cam";
            // 
            // developmentLabel
            // 
            this.developmentLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.developmentLabel.AutoSize = true;
            this.developmentLabel.Enabled = false;
            this.developmentLabel.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.developmentLabel.Location = new System.Drawing.Point(486, 15);
            this.developmentLabel.Name = "developmentLabel";
            this.developmentLabel.Size = new System.Drawing.Size(152, 19);
            this.developmentLabel.Style = MetroFramework.MetroColorStyle.Red;
            this.developmentLabel.TabIndex = 1;
            this.developmentLabel.Text = "DEVELOPMENT BUILD";
            this.developmentLabel.UseStyleColors = true;
            // 
            // authLink
            // 
            this.authLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.authLink.Image = global::ParkitectNexus.Client.Windows.Properties.Resources.appbar_user_tie;
            this.authLink.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.authLink.ImageSize = 32;
            this.authLink.Location = new System.Drawing.Point(584, 37);
            this.authLink.Name = "authLink";
            this.authLink.NoFocusImage = global::ParkitectNexus.Client.Windows.Properties.Resources.appbar_user_tie;
            this.authLink.Size = new System.Drawing.Size(129, 27);
            this.authLink.TabIndex = 2;
            this.authLink.Text = "DEFAULT_VALUE";
            this.authLink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.authLink.UseSelectable = true;
            this.authLink.Click += new System.EventHandler(this.authLink_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackImage = global::ParkitectNexus.Client.Windows.Properties.Resources.parkitectnexus_logo_full;
            this.BackImagePadding = new System.Windows.Forms.Padding(20, 15, 5, 5);
            this.BackMaxSize = 100;
            this.ClientSize = new System.Drawing.Size(722, 450);
            this.Controls.Add(this.authLink);
            this.Controls.Add(this.developmentLabel);
            this.Controls.Add(this.metroTabControl);
            this.DisplayHeader = false;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(450, 450);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(20, 70, 20, 20);
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.DropShadow;
            this.Style = MetroFramework.MetroColorStyle.Orange;
            this.Text = "ParkitectNexus Client";
            this.TextAlign = MetroFramework.Forms.MetroFormTextAlign.Right;
            this.Theme = MetroFramework.MetroThemeStyle.Default;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroTabControl metroTabControl;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroToggle metroToggle1;
        private MetroFramework.Controls.MetroLabel developmentLabel;
        private MetroFramework.Controls.MetroLink authLink;
    }
}

