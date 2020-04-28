using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;

namespace MiShotService
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MiShot()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
