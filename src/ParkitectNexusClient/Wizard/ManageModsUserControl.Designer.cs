namespace ParkitectNexus.Client.Wizard
{
    partial class ManageModsUserControl
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
            this.backButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.modsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.optionsGroupBox = new System.Windows.Forms.GroupBox();
            this.parkitectNexusLinkLabel = new System.Windows.Forms.LinkLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.updateButton = new System.Windows.Forms.Button();
            this.uninstallButton = new System.Windows.Forms.Button();
            this.modVersionLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.modNameLabel = new System.Windows.Forms.Label();
            this.enableModCheckBox = new System.Windows.Forms.CheckBox();
            this.modInDevelopmentLabel = new System.Windows.Forms.Label();
            this.optionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // backButton
            // 
            this.backButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.backButton.Location = new System.Drawing.Point(405, 266);
            this.backButton.Margin = new System.Windows.Forms.Padding(13);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(75, 23);
            this.backButton.TabIndex = 11;
            this.backButton.Text = "< Back";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(266, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Click on a mod to enable, disable, update or uninstall it.";
            // 
            // modsCheckedListBox
            // 
            this.modsCheckedListBox.FormattingEnabled = true;
            this.modsCheckedListBox.HorizontalScrollbar = true;
            this.modsCheckedListBox.Location = new System.Drawing.Point(28, 56);
            this.modsCheckedListBox.Name = "modsCheckedListBox";
            this.modsCheckedListBox.Size = new System.Drawing.Size(228, 184);
            this.modsCheckedListBox.TabIndex = 13;
            this.modsCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.modsCheckedListBox_ItemCheck);
            this.modsCheckedListBox.SelectedIndexChanged += new System.EventHandler(this.modsCheckedListBox_SelectedIndexChanged);
            // 
            // optionsGroupBox
            // 
            this.optionsGroupBox.Controls.Add(this.modInDevelopmentLabel);
            this.optionsGroupBox.Controls.Add(this.parkitectNexusLinkLabel);
            this.optionsGroupBox.Controls.Add(this.label5);
            this.optionsGroupBox.Controls.Add(this.updateButton);
            this.optionsGroupBox.Controls.Add(this.uninstallButton);
            this.optionsGroupBox.Controls.Add(this.modVersionLabel);
            this.optionsGroupBox.Controls.Add(this.label3);
            this.optionsGroupBox.Controls.Add(this.label2);
            this.optionsGroupBox.Controls.Add(this.modNameLabel);
            this.optionsGroupBox.Controls.Add(this.enableModCheckBox);
            this.optionsGroupBox.Location = new System.Drawing.Point(262, 56);
            this.optionsGroupBox.Name = "optionsGroupBox";
            this.optionsGroupBox.Size = new System.Drawing.Size(218, 184);
            this.optionsGroupBox.TabIndex = 14;
            this.optionsGroupBox.TabStop = false;
            this.optionsGroupBox.Text = "Options";
            // 
            // parkitectNexusLinkLabel
            // 
            this.parkitectNexusLinkLabel.AutoSize = true;
            this.parkitectNexusLinkLabel.Location = new System.Drawing.Point(66, 62);
            this.parkitectNexusLinkLabel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.parkitectNexusLinkLabel.Name = "parkitectNexusLinkLabel";
            this.parkitectNexusLinkLabel.Size = new System.Drawing.Size(120, 13);
            this.parkitectNexusLinkLabel.TabIndex = 9;
            this.parkitectNexusLinkLabel.TabStop = true;
            this.parkitectNexusLinkLabel.Text = "View on ParkitectNexus";
            this.parkitectNexusLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.parkitectNexusLinkLabel_LinkClicked);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 62);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Website:";
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(6, 126);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(206, 23);
            this.updateButton.TabIndex = 7;
            this.updateButton.Text = "Check for Updates";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // uninstallButton
            // 
            this.uninstallButton.Location = new System.Drawing.Point(6, 155);
            this.uninstallButton.Name = "uninstallButton";
            this.uninstallButton.Size = new System.Drawing.Size(206, 23);
            this.uninstallButton.TabIndex = 6;
            this.uninstallButton.Text = "Uninstall";
            this.uninstallButton.UseVisualStyleBackColor = true;
            this.uninstallButton.Click += new System.EventHandler(this.uninstallButton_Click);
            // 
            // modVersionLabel
            // 
            this.modVersionLabel.AutoSize = true;
            this.modVersionLabel.Location = new System.Drawing.Point(66, 41);
            this.modVersionLabel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.modVersionLabel.Name = "modVersionLabel";
            this.modVersionLabel.Size = new System.Drawing.Size(55, 13);
            this.modVersionLabel.TabIndex = 5;
            this.modVersionLabel.Text = "VERSION";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 41);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Version:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Mod:";
            // 
            // modNameLabel
            // 
            this.modNameLabel.AutoSize = true;
            this.modNameLabel.Location = new System.Drawing.Point(66, 20);
            this.modNameLabel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.modNameLabel.Name = "modNameLabel";
            this.modNameLabel.Size = new System.Drawing.Size(63, 13);
            this.modNameLabel.TabIndex = 2;
            this.modNameLabel.Text = "MODNAME";
            // 
            // enableModCheckBox
            // 
            this.enableModCheckBox.AutoSize = true;
            this.enableModCheckBox.Location = new System.Drawing.Point(11, 103);
            this.enableModCheckBox.Name = "enableModCheckBox";
            this.enableModCheckBox.Size = new System.Drawing.Size(83, 17);
            this.enableModCheckBox.TabIndex = 0;
            this.enableModCheckBox.Text = "Enable Mod";
            this.enableModCheckBox.UseVisualStyleBackColor = true;
            this.enableModCheckBox.Click += new System.EventHandler(this.enableModCheckBox_Click);
            // 
            // modInDevelopmentLabel
            // 
            this.modInDevelopmentLabel.AutoSize = true;
            this.modInDevelopmentLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modInDevelopmentLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.modInDevelopmentLabel.Location = new System.Drawing.Point(8, 83);
            this.modInDevelopmentLabel.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.modInDevelopmentLabel.Name = "modInDevelopmentLabel";
            this.modInDevelopmentLabel.Size = new System.Drawing.Size(148, 13);
            this.modInDevelopmentLabel.TabIndex = 10;
            this.modInDevelopmentLabel.Text = "MOD IN DEVELOPMENT";
            // 
            // ManageModsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.optionsGroupBox);
            this.Controls.Add(this.modsCheckedListBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.backButton);
            this.Name = "ManageModsUserControl";
            this.optionsGroupBox.ResumeLayout(false);
            this.optionsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox modsCheckedListBox;
        private System.Windows.Forms.GroupBox optionsGroupBox;
        private System.Windows.Forms.CheckBox enableModCheckBox;
        private System.Windows.Forms.Label modNameLabel;
        private System.Windows.Forms.LinkLabel parkitectNexusLinkLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Button uninstallButton;
        private System.Windows.Forms.Label modVersionLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label modInDevelopmentLabel;
    }
}
