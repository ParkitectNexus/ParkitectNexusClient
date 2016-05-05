// ParkitectNexusClient
// Copyright (C) 2016 ParkitectNexus, Tim Potze
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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