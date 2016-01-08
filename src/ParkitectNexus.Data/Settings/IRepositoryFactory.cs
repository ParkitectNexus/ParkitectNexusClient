using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Settings
{
    public interface IRepositoryFactory
    {
        IRepository<T> Repository<T>();
    }
}
