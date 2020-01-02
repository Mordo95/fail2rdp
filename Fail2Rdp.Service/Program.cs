using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Fail2Rdp.Service
{
    static class Program
    {
        public static Settings Settings = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            try
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new LogReadingService()
                };
                ServiceBase.Run(ServicesToRun);
            } catch (Exception e)
            {
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "error.log", e.ToString());
            }
        }
    }
}
