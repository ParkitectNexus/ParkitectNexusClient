// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

namespace ParkitectNexus.Data.Presenter
{
    public interface IPresenterFactory
    {
        T InstantiatePresenter<T>();
    }
}
