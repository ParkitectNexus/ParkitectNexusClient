// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System.Threading.Tasks;
using Xwt;
using Xwt.Drawing;
using Xwt.Motion;

namespace ParkitectNexus.Client.Base.Components
{
    public class SliderBox : VBox
    {
        private const int WideWidth = 280;
        private readonly Button _closeButton;
        private readonly HBox _closeButtonBox;
        private int _slideInRequestsInProgress;

        public SliderBox()
        {
            _closeButtonBox = new HBox();
            _closeButton = new Button("CLOSE")
            {
                Style = ButtonStyle.Flat,
                Image = App.Images["appbar.chevron.left.png"].WithSize(32),
                Font = Font.SystemFont.WithSize(20).WithStretch(FontStretch.UltraCondensed),
                ImagePosition = ContentPosition.Left
            };
            _closeButton.Clicked += (sender, args) => SlideHide();
            _closeButtonBox.PackStart(_closeButton);
            _closeButtonBox.MinWidth = 0;
            _closeButtonBox.WidthRequest = 0;
            PackStart(_closeButtonBox);
        }

        private void RemoveWidthRequest(Widget widget)
        {
            widget.WidthRequest = 0;
            widget.MinWidth = 0;

            var box = widget as Box;
            if (box != null)
                foreach (var w in box.Children)
                    RemoveWidthRequest(w);
        }

        private void Add(Widget widget)
        {
            RemoveWidthRequest(widget);
            PackStart(widget);
        }

        private async Task SlideShowTask(string name, Widget widget)
        {
            _closeButton.Label = name;
            _slideInRequestsInProgress++;
            var requestIndex = _slideInRequestsInProgress;

            await SlideHideTask(100);

            while (this.AnimationIsRunning("show") || this.AnimationIsRunning("hide"))
            {
                if (this.AnimationIsRunning("show"))
                    await SlideHideTask(100);

                while (this.AnimationIsRunning("hide"))
                    await Task.Delay(10);

                if (requestIndex > _slideInRequestsInProgress)
                {
                    _slideInRequestsInProgress--;
                    return;
                }
                requestIndex = _slideInRequestsInProgress;
            }

            if (widget != null)
                Add(widget);

            this.Animate("show", r => MinWidth = r, 0, WideWidth, 1, 450, Easing.SinOut);
            _slideInRequestsInProgress--;
        }

        private async Task SlideHideTask(uint time)
        {
            if (MinWidth <= 0)
                return;

            if (this.AnimationIsRunning("hide"))
                return;

            while (this.AnimationIsRunning("show"))
                await Task.Delay(10);

            this.Animate("hide", r => MinWidth = r, MinWidth, 0, 1, time, Easing.SinOut, (d, b) =>
            {
                Clear();
                PackStart(_closeButtonBox);
            });

            while (this.AnimationIsRunning("hide"))
                await Task.Delay(10);
        }

        public async void SlideHide()
        {
            await SlideHideTask(450);
        }

        public async void SlideShow(string name, Widget widget)
        {
            await SlideShowTask(name, widget);
        }
    }
}
