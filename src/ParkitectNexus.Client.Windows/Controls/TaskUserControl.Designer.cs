namespace ParkitectNexus.Client.Windows.Controls
{
    partial class TaskUserControl
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
            this.progressSpinner = new MetroFramework.Controls.MetroProgressSpinner();
            this.descriptionLabel = new MetroFramework.Controls.MetroLabel();
            this.donePictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.donePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.nameLabel.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.nameLabel.Location = new System.Drawing.Point(3, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(104, 25);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Task Name";
            // 
            // progressSpinner
            // 
            this.progressSpinner.Location = new System.Drawing.Point(3, 28);
            this.progressSpinner.Maximum = 100;
            this.progressSpinner.Name = "progressSpinner";
            this.progressSpinner.Size = new System.Drawing.Size(32, 32);
            this.progressSpinner.Speed = 0.1F;
            this.progressSpinner.Style = MetroFramework.MetroColorStyle.Black;
            this.progressSpinner.TabIndex = 1;
            this.progressSpinner.UseSelectable = true;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(34, 25);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(101, 19);
            this.descriptionLabel.TabIndex = 3;
            this.descriptionLabel.Text = "Task Description";
            // 
            // donePictureBox
            // 
            this.donePictureBox.BackColor = System.Drawing.Color.Transparent;
            this.donePictureBox.Image = global::ParkitectNexus.Client.Windows.Properties.Resources.appbar_check;
            this.donePictureBox.Location = new System.Drawing.Point(3, 28);
            this.donePictureBox.Name = "donePictureBox";
            this.donePictureBox.Size = new System.Drawing.Size(32, 32);
            this.donePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.donePictureBox.TabIndex = 4;
            this.donePictureBox.TabStop = false;
            // 
            // TaskUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.donePictureBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.progressSpinner);
            this.Controls.Add(this.nameLabel);
            this.Name = "TaskUserControl";
            this.Size = new System.Drawing.Size(606, 69);
            ((System.ComponentModel.ISupportInitialize)(this.donePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel nameLabel;
        private MetroFramework.Controls.MetroProgressSpinner progressSpinner;
        private MetroFramework.Controls.MetroLabel descriptionLabel;
        private System.Windows.Forms.PictureBox donePictureBox;
    }
}
