// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Client.Base.Components;
using ParkitectNexus.Client.Base.Pages;
using ParkitectNexus.Client.Base.Tiles;
using ParkitectNexus.Data.Presenter;
using Xwt;

namespace ParkitectNexus.Client.Base.Main
{
    public class MainView : VBox, IPresenter
    {
        private readonly MainNotebook _notebook;
        private readonly SidebarContainer _sidebarContainer;

        public MainView(IPresenterFactory presenterFactory)
        {
            _notebook = presenterFactory.InstantiatePresenter<MainNotebook>();
            _notebook.Add(presenterFactory.InstantiatePresenter<MenuPageView>(this));
            _notebook.Add(presenterFactory.InstantiatePresenter<ModsPageView>(this));
            _notebook.Add(presenterFactory.InstantiatePresenter<BlueprintsPageView>(this));
            _notebook.Add(presenterFactory.InstantiatePresenter<SavegamesPageView>(this));
            _notebook.Add(presenterFactory.InstantiatePresenter<TasksPageView>(this));

            PackStart(presenterFactory.InstantiatePresenter<MainHeaderView>());

            var sideBox = new VBox
            {
                MinWidth = 280,
                WidthRequest = 280
            };

            _sidebarContainer = new SidebarContainer();
            sideBox.PackStart(_sidebarContainer, true, true);
            var box = new HBox();

            box.PackStart(_notebook, true);
            box.PackEnd(sideBox);

            PackStart(box, true, true);

            _notebook.HandleSizeChangeOnTabChange = true;
            _notebook.HandleSizeUpdate();
        }

        public void SwitchToTab(int index)
        {
            _notebook.CurrentTabIndex = index;
        }

        public void ShowSidebarWidget(string name, Widget widget)
        {
            _sidebarContainer.ShowWidget(name, widget);
        }

        protected override void OnBoundsChanged()
        {
            base.OnBoundsChanged();

            _notebook.HandleSizeUpdate();
        }
    }
}
