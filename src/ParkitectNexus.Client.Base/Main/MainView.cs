// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Client.Base.Components;
using ParkitectNexus.Client.Base.Pages;
using ParkitectNexus.Data.Presenter;
using Xwt;
using ParkitectNexus.Client.Base.Tiles;

namespace ParkitectNexus.Client.Base.Main
{
    public class MainView : VBox, IPresenter
    {
        private readonly MainNotebook _notebook;
        private readonly SliderBox _sliderBox;

        public MainView(IPresenterFactory presenterFactory)
        {
            _notebook = presenterFactory.InstantiatePresenter<MainNotebook>();
            _notebook.Add(presenterFactory.InstantiatePresenter<MenuPageView>(this));
            _notebook.Add(presenterFactory.InstantiatePresenter<SavegamesPageView>(this));
            PackStart(presenterFactory.InstantiatePresenter<MainHeaderView>());

            var box = new HBox();
            box.PackStart(_notebook, true);
            box.PackEnd(_sliderBox = new SliderBox());

            PackEnd(box, true, true);
        }

        public void ShowSidebarWidget(string name, Widget widget)
        {
            _sliderBox.SlideShow(name, widget);
        }

        protected override void OnBoundsChanged()
        {
            base.OnBoundsChanged();

            var tv = _notebook.CurrentTab.Child as LoadableDataTileView;
            if(tv != null)
                tv.HandleSizeUpdate();
        }
    }
}
