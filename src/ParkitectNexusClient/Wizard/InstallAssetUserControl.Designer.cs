namespace ParkitectNexus.Client.Wizard
{
    partial class InstallAssetUserControl
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
            this.components = new System.ComponentModel.Container();
            this.finishButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.statusIsLabel = new System.Windows.Forms.Label();
            this.installingLabel = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.downloadingTimer = new System.Windows.Forms.Timer(this.components);
            this.closeTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // finishButton
            // 
            this.finishButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.finishButton.Enabled = false;
            this.finishButton.Location = new System.Drawing.Point(405, 266);
            this.finishButton.Margin = new System.Windows.Forms.Padding(13);
            this.finishButton.Name = "finishButton";
            this.finishButton.Size = new System.Drawing.Size(75, 23);
            this.finishButton.TabIndex = 10;
            this.finishButton.Text = "Finish";
            this.finishButton.UseVisualStyleBackColor = true;
            this.finishButton.Click += new System.EventHandler(this.finishButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(93, 110);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(72, 13);
            this.statusLabel.TabIndex = 9;
            this.statusLabel.Text = "Downloading.";
            // 
            // statusIsLabel
            // 
            this.statusIsLabel.AutoSize = true;
            this.statusIsLabel.Location = new System.Drawing.Point(26, 110);
            this.statusIsLabel.Name = "statusIsLabel";
            this.statusIsLabel.Size = new System.Drawing.Size(40, 13);
            this.statusIsLabel.TabIndex = 8;
            this.statusIsLabel.Text = "Status:";
            // 
            // installingLabel
            // 
            this.installingLabel.AutoSize = true;
            this.installingLabel.Location = new System.Drawing.Point(26, 62);
            this.installingLabel.Name = "installingLabel";
            this.installingLabel.Size = new System.Drawing.Size(65, 13);
            this.installingLabel.TabIndex = 7;
            this.installingLabel.Text = "Installing {0}";
            // 
            // progressBar
            // 
            this.progressBar.Cursor = System.Windows.Forms.Cursors.Default;
            this.progressBar.Location = new System.Drawing.Point(26, 128);
            this.progressBar.MarqueeAnimationSpeed = 10;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(440, 13);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 6;
            this.progressBar.Value = 100;
            // 
            // downloadingTimer
            // 
            this.downloadingTimer.Enabled = true;
            this.downloadingTimer.Interval = 20;
            this.downloadingTimer.Tick += new System.EventHandler(this.downloadingTimer_Tick);
            // 
            // closeTimer
            // 
            this.closeTimer.Interval = 2500;
            this.closeTimer.Tick += new System.EventHandler(this.closeTimer_Tick);
            // 
            // InstallAssetUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.finishButton);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.statusIsLabel);
            this.Controls.Add(this.installingLabel);
            this.Controls.Add(this.progressBar);
            this.Name = "InstallAssetUserControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button finishButton;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label statusIsLabel;
        private System.Windows.Forms.Label installingLabel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Timer downloadingTimer;
        private System.Windows.Forms.Timer closeTimer;
    }
}
