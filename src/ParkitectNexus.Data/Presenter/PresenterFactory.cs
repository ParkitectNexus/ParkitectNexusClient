using ParkitectNexus.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Presenter
{
    public class PresenterFactory : IPresenterFactory
    {
        public T InstantiatePresenter<T>()
        {
            return ObjectFactory.Container.GetInstance<T>();
        }

        public T InstantiatePresenter<T>(IPresenter parent)
        {
            return ObjectFactory.Container.With (parent).GetInstance<T> ();
        }

    }
}
