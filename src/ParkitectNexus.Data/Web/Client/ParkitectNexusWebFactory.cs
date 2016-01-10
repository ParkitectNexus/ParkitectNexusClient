using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Web.Client
{
    public class ParkitectNexusWebFactory : IParkitectNexusWebFactory
    {
        public IParkitectNexusWeb NexusClient()
        {
            return ObjectFactory.Container.GetInstance<IParkitectNexusWeb>();
        }
        
    }
}
