// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using System;
using System.Linq;
using ParkitectNexus.Client.Base.Pages;
using ParkitectNexus.Client.Base.Tiles;
using ParkitectNexus.Data.Presenter;
using Xwt;

namespace ParkitectNexus.Client.Base.Main
{
    public class MainNotebook : Notebook, IPresenter
    {
        public MainNotebook()
        {
            MinWidth = 100;
            WidthRequest = 100;
        }

        public bool HandleSizeChangeOnTabChange { get; set; }

        public void Add(Widget w)
        {
            var page = w as IPageView;

            if (page != null)
            {
                Add(w, page.DisplayName);

                page.DisplayNameChanged += Page_NameChanged;
            }
        }


        private void Page_NameChanged(object sender, EventArgs e)
        {
            var tab = Tabs.FirstOrDefault(t => t.Child == sender);
            var page = tab?.Child as IPageView;

            if (tab != null && page != null)
                Application.Invoke(() => { tab.Label = page.DisplayName; });
        }

        public void HandleSizeUpdate()
        {
            foreach(var tab in Tabs)
            {
                var loadableTilesView = tab.Child as LoadableDataTileView;

                loadableTilesView?.HandleSizeUpdate((float)Size.Width);

                var vvv = tab.Child as TasksPageView;
                vvv?.HandleSizeChange();
            }
        }

        #region Overrides of Notebook

        protected override void OnCurrentTabChanged(EventArgs e)
        {
            base.OnCurrentTabChanged(e);

            if(!HandleSizeChangeOnTabChange)
                return;

            HandleSizeUpdate();
//            var loadableTilesView = CurrentTab.Child as LoadableDataTileView;
//
//            loadableTilesView?.HandleSizeUpdate((float)Size.Width);
        }

        #endregion
    }
}
