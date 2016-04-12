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
        private readonly SidebarContainer _sidebarContainer;

        public MainView(IPresenterFactory presenterFactory)
        {
            var tabcon = presenterFactory.InstantiatePresenter<MainNotebook>();
            tabcon.Add(presenterFactory.InstantiatePresenter<MenuPageView>(this));
            tabcon.Add(presenterFactory.InstantiatePresenter<BlueprintsPageView>(this));
            tabcon.Add(presenterFactory.InstantiatePresenter<SavegamesPageView>(this));

            PackStart(presenterFactory.InstantiatePresenter<MainHeaderView>());

            var sideBox = new VBox
            {
                MinWidth = 280,
                WidthRequest = 280
            };

            _sidebarContainer = new SidebarContainer();
            sideBox.PackStart(_sidebarContainer, true, true);
            var box = new HBox();
            box.PackStart(tabcon, true);
            box.PackEnd(sideBox);

            PackStart(box, true, true);
        }

        public void ShowSidebarWidget(string name, Widget widget)
        {
            _sidebarContainer.ShowWidget(name, widget);
        }
    }
}
