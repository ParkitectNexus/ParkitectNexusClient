namespace ParkitectNexus.Client.Windows.Controls.SliderPanels
{
    partial class AssetSliderPanel
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
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.uploadButton = new MetroFramework.Controls.MetroButton();
            this.deleteButton = new MetroFramework.Controls.MetroButton();
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
            this.nameLabel.Size = new System.Drawing.Size(146, 25);
            this.nameLabel.TabIndex = 5;
            this.nameLabel.Text = "Savegame Name";
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(23, 87);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(233, 233);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 6;
            this.pictureBox.TabStop = false;
            // 
            // uploadButton
            // 
            this.uploadButton.Location = new System.Drawing.Point(23, 326);
            this.uploadButton.Name = "uploadButton";
            this.uploadButton.Size = new System.Drawing.Size(233, 23);
            this.uploadButton.TabIndex = 7;
            this.uploadButton.Text = "Upload";
            this.uploadButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.uploadButton.UseSelectable = true;
            this.uploadButton.Visible = false;
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(23, 355);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(233, 23);
            this.deleteButton.TabIndex = 8;
            this.deleteButton.Text = "Delete";
            this.deleteButton.UseSelectable = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // AssetSliderPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackText = "Savegame";
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.uploadButton);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.nameLabel);
            this.Name = "AssetSliderPanel";
            this.Size = new System.Drawing.Size(279, 483);
            this.Controls.SetChildIndex(this.nameLabel, 0);
            this.Controls.SetChildIndex(this.pictureBox, 0);
            this.Controls.SetChildIndex(this.uploadButton, 0);
            this.Controls.SetChildIndex(this.deleteButton, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroLabel nameLabel;
        private System.Windows.Forms.PictureBox pictureBox;
        private MetroFramework.Controls.MetroButton uploadButton;
        private MetroFramework.Controls.MetroButton deleteButton;
    }
}
