namespace ParkitectNexus.Client.Windows.Controls.SliderPanels
{
    partial class ModSliderPanel
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
            this.nameLabel = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.enableModToggle = new MetroFramework.Controls.MetroToggle();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.versionLabel = new MetroFramework.Controls.MetroLabel();
            this.latestVersionLabel = new MetroFramework.Controls.MetroLabel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.deleteButton = new MetroFramework.Controls.MetroButton();
            this.updateButton = new MetroFramework.Controls.MetroButton();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.debugTextBox = new MetroFramework.Controls.MetroTextBox();
            this.recompileButton = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.nameLabel.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.nameLabel.Location = new System.Drawing.Point(23, 59);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(102, 25);
            this.nameLabel.TabIndex = 6;
            this.nameLabel.Text = "Mod Name";
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(23, 88);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(51, 19);
            this.metroLabel1.TabIndex = 8;
            this.metroLabel1.Text = "Version";
            // 
            // enableModToggle
            // 
            this.enableModToggle.AutoSize = true;
            this.enableModToggle.Location = new System.Drawing.Point(176, 135);
            this.enableModToggle.Name = "enableModToggle";
            this.enableModToggle.Size = new System.Drawing.Size(80, 17);
            this.enableModToggle.TabIndex = 9;
            this.enableModToggle.Text = "Off";
            this.enableModToggle.UseSelectable = true;
            this.enableModToggle.CheckedChanged += new System.EventHandler(this.enableModToggle_CheckedChanged);
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(23, 134);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(56, 19);
            this.metroLabel2.TabIndex = 10;
            this.metroLabel2.Text = "Enabled";
            // 
            // versionLabel
            // 
            this.versionLabel.Location = new System.Drawing.Point(80, 88);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(176, 19);
            this.versionLabel.TabIndex = 11;
            this.versionLabel.Text = "-";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // latestVersionLabel
            // 
            this.latestVersionLabel.Location = new System.Drawing.Point(117, 111);
            this.latestVersionLabel.Name = "latestVersionLabel";
            this.latestVersionLabel.Size = new System.Drawing.Size(139, 19);
            this.latestVersionLabel.TabIndex = 15;
            this.latestVersionLabel.Text = "-";
            this.latestVersionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(23, 111);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(88, 19);
            this.metroLabel4.TabIndex = 14;
            this.metroLabel4.Text = "Latest Version";
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(23, 455);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(233, 23);
            this.deleteButton.TabIndex = 17;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseSelectable = true;
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(142, 426);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(114, 23);
            this.updateButton.TabIndex = 16;
            this.updateButton.Text = "Update";
            this.updateButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.updateButton.UseSelectable = true;
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(23, 397);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(233, 23);
            this.metroButton1.TabIndex = 18;
            this.metroButton1.Text = "View on ParkitectNexus";
            this.metroButton1.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroButton1.UseSelectable = true;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(23, 158);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(233, 233);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 19;
            this.pictureBox.TabStop = false;
            // 
            // debugTextBox
            // 
            // 
            // 
            // 
            this.debugTextBox.CustomButton.Image = null;
            this.debugTextBox.CustomButton.Location = new System.Drawing.Point(143, 1);
            this.debugTextBox.CustomButton.Name = "";
            this.debugTextBox.CustomButton.Size = new System.Drawing.Size(89, 89);
            this.debugTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.debugTextBox.CustomButton.TabIndex = 1;
            this.debugTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.debugTextBox.CustomButton.UseSelectable = true;
            this.debugTextBox.CustomButton.Visible = false;
            this.debugTextBox.Lines = new string[] {
        "debugTextBox"};
            this.debugTextBox.Location = new System.Drawing.Point(23, 484);
            this.debugTextBox.MaxLength = 32767;
            this.debugTextBox.Multiline = true;
            this.debugTextBox.Name = "debugTextBox";
            this.debugTextBox.PasswordChar = '\0';
            this.debugTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.debugTextBox.SelectedText = "";
            this.debugTextBox.SelectionLength = 0;
            this.debugTextBox.SelectionStart = 0;
            this.debugTextBox.Size = new System.Drawing.Size(233, 91);
            this.debugTextBox.TabIndex = 20;
            this.debugTextBox.Text = "debugTextBox";
            this.debugTextBox.UseSelectable = true;
            this.debugTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.debugTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // recompileButton
            // 
            this.recompileButton.Location = new System.Drawing.Point(23, 426);
            this.recompileButton.Name = "recompileButton";
            this.recompileButton.Size = new System.Drawing.Size(114, 23);
            this.recompileButton.TabIndex = 21;
            this.recompileButton.Text = "Recompile";
            this.recompileButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.recompileButton.UseSelectable = true;
            this.recompileButton.Click += new System.EventHandler(this.recompileButton_Click);
            // 
            // ModSliderPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackText = "Mod";
            this.Controls.Add(this.recompileButton);
            this.Controls.Add(this.debugTextBox);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.latestVersionLabel);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.enableModToggle);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.nameLabel);
            this.Name = "ModSliderPanel";
            this.Size = new System.Drawing.Size(279, 612);
            this.Controls.SetChildIndex(this.nameLabel, 0);
            this.Controls.SetChildIndex(this.metroLabel1, 0);
            this.Controls.SetChildIndex(this.enableModToggle, 0);
            this.Controls.SetChildIndex(this.metroLabel2, 0);
            this.Controls.SetChildIndex(this.versionLabel, 0);
            this.Controls.SetChildIndex(this.metroLabel4, 0);
            this.Controls.SetChildIndex(this.latestVersionLabel, 0);
            this.Controls.SetChildIndex(this.updateButton, 0);
            this.Controls.SetChildIndex(this.deleteButton, 0);
            this.Controls.SetChildIndex(this.metroButton1, 0);
            this.Controls.SetChildIndex(this.pictureBox, 0);
            this.Controls.SetChildIndex(this.debugTextBox, 0);
            this.Controls.SetChildIndex(this.recompileButton, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel nameLabel;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroToggle enableModToggle;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel versionLabel;
        private MetroFramework.Controls.MetroLabel latestVersionLabel;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroButton deleteButton;
        private MetroFramework.Controls.MetroButton updateButton;
        private MetroFramework.Controls.MetroButton metroButton1;
        private System.Windows.Forms.PictureBox pictureBox;
        private MetroFramework.Controls.MetroTextBox debugTextBox;
        private MetroFramework.Controls.MetroButton recompileButton;
    }
}
