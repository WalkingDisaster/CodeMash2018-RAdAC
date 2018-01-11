using System;
using System.ComponentModel;
using System.Threading.Tasks;
using LogGenerator.Logging;

namespace LogGenerator.Actors
{
    public interface IActor
    {
        void Do(DateTime localNow, Logger logger);
    }
}