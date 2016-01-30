namespace ParkitectNexus.Client.Windows.Controls.SliderPanels
{
    partial class SliderPanel
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
            this.metroLink = new MetroFramework.Controls.MetroLink();
            this.SuspendLayout();
            // 
            // metroLink
            // 
            this.metroLink.FontSize = MetroFramework.MetroLinkSize.Tall;
            this.metroLink.FontWeight = MetroFramework.MetroLinkWeight.Light;
            this.metroLink.Image = global::ParkitectNexus.Client.Windows.Properties.Resources.appbar_chevron_left;
            this.metroLink.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.metroLink.ImageSize = 32;
            this.metroLink.Location = new System.Drawing.Point(5, 5);
            this.metroLink.Name = "metroLink";
            this.metroLink.Size = new System.Drawing.Size(117, 23);
            this.metroLink.TabIndex = 0;
            this.metroLink.Text = "metroLink";
            this.metroLink.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.metroLink.UseSelectable = true;
            this.metroLink.Click += new System.EventHandler(this.metroLink_Click);
            // 
            // SliderPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.metroLink);
            this.Name = "SliderPanel";
            this.Padding = new System.Windows.Forms.Padding(20);
            this.Size = new System.Drawing.Size(279, 150);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroLink metroLink;
    }
}
