// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using Xwt;
using Xwt.Drawing;

namespace ParkitectNexus.Client.Base.Components
{
    public class SidebarContainer : VBox
    {
        private const int WideWidth = 280;
        private readonly Button _closeButton;
        private readonly HBox _closeButtonBox;

        public SidebarContainer()
        {
            _closeButtonBox = new HBox();
            _closeButton = new Button("CLOSE")
            {
                Style = ButtonStyle.Flat,
                Image = App.Images["appbar.chevron.left.png"].WithSize(32),
                Font = Font.SystemFont.WithSize(20).WithStretch(FontStretch.UltraCondensed),
                ImagePosition = ContentPosition.Left
            };
            _closeButton.Clicked += (sender, args) => Clear();
            _closeButtonBox.PackStart(_closeButton);
        }

        public void ShowWidget(string name, Widget widget)
        {
            Clear();

            _closeButton.Label = name;

            if (widget != null)
            {
                PackStart(_closeButtonBox);
                PackStart(widget);
            }
        }
    }
}
