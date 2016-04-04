// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Data.Presenter;
using Xwt;

namespace ParkitectNexus.Client.Base.Main
{
    public class MainHeaderView : HBox, IPresenter
    {
        public MainHeaderView()
        {
            PackStart(new ImageView(App.Images["parkitectnexus_logo_full.png"]));
        }
    }
}
