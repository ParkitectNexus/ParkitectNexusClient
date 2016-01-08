using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Presenter
{
    public class PresenterRegistry : Registry
    {
        public PresenterRegistry()
        {
            Scan(x =>
            {
                x.IncludeNamespaceContainingType<IPresenter>();
                x.WithDefaultConventions();
            });

            For<IPresenterFactory>().Use<PresenterFactory>();

        }
    }
}
