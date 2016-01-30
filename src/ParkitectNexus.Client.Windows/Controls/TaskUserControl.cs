﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Drawing;
using ParkitectNexus.Data.Tasks;
using ParkitectNexus.Data.Utilities;
using TaskStatus = ParkitectNexus.Data.Tasks.TaskStatus;

namespace ParkitectNexus.Client.Windows.Controls
{
    public partial class TaskUserControl : MetroUserControl
    {
        private readonly IQueueableTask _task;

        public TaskUserControl(IQueueableTask task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            _task = task;

            _task.StatusChanged += _task_StatusChanged;
            InitializeComponent();

            donePictureBox.Visible = false;
            donePictureBox.Image = ImageUtility.RecolorImage(donePictureBox.Image, MetroColors.Green);
            UpdateUI();
        }

        private void _task_StatusChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            nameLabel.Text = _task.Name;
            descriptionLabel.Text = _task.StatusDescription;
            progressSpinner.Value = _task.CompletionPercentage;

            switch (_task.Status)
            {
                case TaskStatus.Queued:
                    progressSpinner.Style = MetroColorStyle.Black;
                    progressSpinner.Speed = 0.1f;
                    break;
                case TaskStatus.Stopped:
                    progressSpinner.Visible = false;
                    donePictureBox.Visible = true;
                    break;
                case TaskStatus.Canceled:
                    progressSpinner.Style = MetroColorStyle.Silver;
                    break;
                case TaskStatus.Running:
                    progressSpinner.Style = MetroColorStyle.Blue;
                    progressSpinner.Speed = 1;
                    break;
            }
        }

        #region Overrides of MetroUserControl

        protected override void OnCustomPaintForeground(MetroPaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0xcc, 0xcc, 0xcc)), 0, Height - 2, Width, 2);
            base.OnCustomPaintForeground(e);
        }

        #endregion
    }
}
