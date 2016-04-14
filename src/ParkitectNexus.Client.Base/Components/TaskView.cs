// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using ParkitectNexus.Data.Tasks;
using Xwt;
using Xwt.Drawing;

namespace ParkitectNexus.Client.Base.Components
{
    public class TaskView : VBox
    {
        private readonly Label _descriptionlabel;

        private readonly Label _nameLabel;
        private readonly Spinner _spinner;

        private bool _isSpinning;

        public TaskView(IQueueableTask task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            Task = task;
            task.StatusChanged += Task_StatusChanged;

            _nameLabel = new Label
            {
                Font = Font.SystemFont.WithWeight(FontWeight.Bold).WithSize(15)
            };
            _descriptionlabel = new Label
            {
                Font = Font.SystemFont.WithStyle(FontStyle.Italic)
            };

            _spinner = new Spinner {Visible = false};

            var hBox = new HBox();
            hBox.PackStart(_spinner);
            hBox.PackStart(_descriptionlabel);

            PackStart(_nameLabel);
            PackStart(hBox);

            HeightRequest = 64;
            MinHeight = 64;

            UpdateLabels();
        }

        public IQueueableTask Task { get; }

        private void UpdateLabels()
        {
            Application.Invoke(() =>
            {
                switch (Task.Status)
                {
                    case TaskStatus.Running:
                        if (!_isSpinning)
                        {
                            _spinner.Visible = true;
                            _spinner.Animate = true;
                            _isSpinning = true;
                        }
                        break;
                    case TaskStatus.Break:
                    case TaskStatus.FinishedWithErrors:
                    case TaskStatus.Canceled:
                    case TaskStatus.Finished:
                    case TaskStatus.Queued:
                        if (_isSpinning)
                        {
                            _spinner.Visible = false;
                            _spinner.Animate = false;
                            _isSpinning = false;
                        }
                        break;
                }
                switch (Task.Status)
                {
                    case TaskStatus.Queued:
                    case TaskStatus.Running:
                    case TaskStatus.Break:
                        _nameLabel.TextColor = Colors.Black;
                        break;
                    case TaskStatus.FinishedWithErrors:
                        _nameLabel.TextColor = Colors.DarkRed;
                        break;
                    case TaskStatus.Canceled:
                        _nameLabel.TextColor = Colors.DarkGray;
                        break;
                    case TaskStatus.Finished:
                        _nameLabel.TextColor = Colors.DarkOliveGreen;
                        break;
                }


                _nameLabel.Text = Task.Name;
                _descriptionlabel.Text = Task.StatusDescription;
            });
        }

        private void Task_StatusChanged(object sender, EventArgs e)
        {
            UpdateLabels();
        }
    }
}
