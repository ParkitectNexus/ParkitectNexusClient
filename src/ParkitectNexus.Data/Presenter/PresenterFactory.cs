// ParkitectNexusClient
// Copyright 2016 Parkitect, Tim Potze

namespace ParkitectNexus.Data.Presenter
{
    public class PresenterFactory : IPresenterFactory
    {
        public T InstantiatePresenter<T>()
        {
            return ObjectFactory.GetInstance<T>();
        }
    }
}
