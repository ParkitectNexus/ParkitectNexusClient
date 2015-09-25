namespace ParkitectNexus.Client.Wizard
{
    partial class MenuUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.closeButton = new System.Windows.Forms.Button();
            this.manageModsButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.launchParkitectButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.visitParkitectNexusButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(405, 266);
            this.closeButton.Margin = new System.Windows.Forms.Padding(13);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 11;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // manageModsButton
            // 
            this.manageModsButton.Location = new System.Drawing.Point(55, 29);
            this.manageModsButton.Name = "manageModsButton";
            this.manageModsButton.Size = new System.Drawing.Size(120, 23);
            this.manageModsButton.TabIndex = 12;
            this.manageModsButton.Text = "Manage Mods";
            this.manageModsButton.UseVisualStyleBackColor = true;
            this.manageModsButton.Click += new System.EventHandler(this.manageModsButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(85, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Disable, update or uninstall your installed mods.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(85, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(218, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Launch Parkitect with all your enabled mods.";
            // 
            // launchParkitectButton
            // 
            this.launchParkitectButton.Location = new System.Drawing.Point(55, 108);
            this.launchParkitectButton.Name = "launchParkitectButton";
            this.launchParkitectButton.Size = new System.Drawing.Size(120, 23);
            this.launchParkitectButton.TabIndex = 14;
            this.launchParkitectButton.Text = "Launch Parkitect";
            this.launchParkitectButton.UseVisualStyleBackColor = true;
            this.launchParkitectButton.Click += new System.EventHandler(this.launchParkitectButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(85, 221);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(368, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Visit ParkitectNexus.com to download new mods, blueprints and savegames.";
            // 
            // visitParkitectNexusButton
            // 
            this.visitParkitectNexusButton.Location = new System.Drawing.Point(55, 188);
            this.visitParkitectNexusButton.Name = "visitParkitectNexusButton";
            this.visitParkitectNexusButton.Size = new System.Drawing.Size(120, 23);
            this.visitParkitectNexusButton.TabIndex = 16;
            this.visitParkitectNexusButton.Text = "Visit ParkitectNexus";
            this.visitParkitectNexusButton.UseVisualStyleBackColor = true;
            this.visitParkitectNexusButton.Click += new System.EventHandler(this.visitParkitectNexusButton_Click);
            // 
            // MenuUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.visitParkitectNexusButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.launchParkitectButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.manageModsButton);
            this.Controls.Add(this.closeButton);
            this.Name = "MenuUserControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button manageModsButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button launchParkitectButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button visitParkitectNexusButton;
    }
}
