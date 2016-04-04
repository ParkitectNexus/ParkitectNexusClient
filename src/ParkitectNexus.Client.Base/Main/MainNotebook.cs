// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

using ParkitectNexus.Data.Presenter;
using Xwt;

namespace ParkitectNexus.Client.Base.Main
{
    public class MainNotebook : Notebook, IPresenter
    {
        public void Add(Widget w)
        {
            Add(w, w.Name);
        }
    }
}
