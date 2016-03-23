using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkitectNexus.Client.Linux
{
    public interface IPage
    {
        void OnOpen();
        void OnClose();
    }
}
