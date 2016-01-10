using ParkitectNexus.Data.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkitectNexus.Data.Reporting
{
    public interface ICrashReporterFactory
    {
        void Report(string action, Exception exception);
    }
}
