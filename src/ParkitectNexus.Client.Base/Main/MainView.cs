// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Client.Base.Components;
using ParkitectNexus.Client.Base.Pages;
using ParkitectNexus.Data.Presenter;
using Xwt;

namespace ParkitectNexus.Client.Base.Main
{
    public class MainView : VBox, IPresenter
    {
        private readonly SliderBox _sliderBox;

        public MainView(IPresenterFactory presenterFactory)
        {
            var tabcon = presenterFactory.InstantiatePresenter<MainNotebook>();
            tabcon.Add(presenterFactory.InstantiatePresenter<MenuPageView>(this));
            tabcon.Add(presenterFactory.InstantiatePresenter<SavegamesPageView>(this));
            PackStart(presenterFactory.InstantiatePresenter<MainHeaderView>());

            var box = new HBox();
            box.PackStart(tabcon, true);
            box.PackEnd(_sliderBox = new SliderBox());

            PackEnd(box, true, true);
        }

        public void ShowSidebarWidget(string name, Widget widget)
        {
            _sliderBox.SlideShow(name, widget);
        }
    }
}
