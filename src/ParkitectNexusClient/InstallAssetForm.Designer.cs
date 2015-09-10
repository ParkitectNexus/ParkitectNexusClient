namespace ParkitectNexusClient
{
    partial class InstallAssetForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallAssetForm));
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.installingLabel = new System.Windows.Forms.Label();
            this.statusIsLabel = new System.Windows.Forms.Label();
            this.downloadingTimer = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.finishButton = new System.Windows.Forms.Button();
            this.closeTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Cursor = System.Windows.Forms.Cursors.Default;
            this.progressBar.Location = new System.Drawing.Point(26, 153);
            this.progressBar.MarqueeAnimationSpeed = 10;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(440, 13);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 0;
            this.progressBar.Value = 100;
            // 
            // installingLabel
            // 
            this.installingLabel.AutoSize = true;
            this.installingLabel.Location = new System.Drawing.Point(26, 87);
            this.installingLabel.Name = "installingLabel";
            this.installingLabel.Size = new System.Drawing.Size(65, 13);
            this.installingLabel.TabIndex = 1;
            this.installingLabel.Text = "Installing {0}";
            // 
            // statusIsLabel
            // 
            this.statusIsLabel.AutoSize = true;
            this.statusIsLabel.Location = new System.Drawing.Point(26, 135);
            this.statusIsLabel.Name = "statusIsLabel";
            this.statusIsLabel.Size = new System.Drawing.Size(40, 13);
            this.statusIsLabel.TabIndex = 2;
            this.statusIsLabel.Text = "Status:";
            // 
            // downloadingTimer
            // 
            this.downloadingTimer.Enabled = true;
            this.downloadingTimer.Interval = 20;
            this.downloadingTimer.Tick += new System.EventHandler(this.downloadingTimer_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::ParkitectNexusClient.Properties.Resources.dialog_banner;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(477, 58);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(93, 135);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(72, 13);
            this.statusLabel.TabIndex = 4;
            this.statusLabel.Text = "Downloading.";
            // 
            // finishButton
            // 
            this.finishButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.finishButton.Enabled = false;
            this.finishButton.Location = new System.Drawing.Point(390, 286);
            this.finishButton.Name = "finishButton";
            this.finishButton.Size = new System.Drawing.Size(75, 23);
            this.finishButton.TabIndex = 5;
            this.finishButton.Text = "Finish";
            this.finishButton.UseVisualStyleBackColor = true;
            this.finishButton.Click += new System.EventHandler(this.finishButton_Click);
            // 
            // closeTimer
            // 
            this.closeTimer.Interval = 2500;
            this.closeTimer.Tick += new System.EventHandler(this.closeTimer_Tick);
            // 
            // InstallAssetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 321);
            this.Controls.Add(this.finishButton);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.statusIsLabel);
            this.Controls.Add(this.installingLabel);
            this.Controls.Add(this.progressBar);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InstallAssetForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parkitect Nexus Client";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label installingLabel;
        private System.Windows.Forms.Label statusIsLabel;
        private System.Windows.Forms.Timer downloadingTimer;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button finishButton;
        private System.Windows.Forms.Timer closeTimer;
    }
}

