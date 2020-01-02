using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Fail2Rdp.ServiceHelper
{
    public static class ServiceHelper
    {
        private const string SERVICE_EXE = "Fail2Rdp.Service.exe";
        private const string SERVICE_NAME = "Fail 2 RDP";

        public static bool RemoveService()
        {
            string dotNetPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "InstallUtil.exe";
                p.StartInfo.WorkingDirectory = dotNetPath;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.Arguments = "-u " + Path.Combine(Environment.CurrentDirectory, SERVICE_EXE);
                p.Start();
                p.WaitForExit();
                return p.ExitCode == 0;
            }
        }

        public static bool InstallService()
        {
            string dotNetPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "InstallUtil.exe";
                p.StartInfo.WorkingDirectory = dotNetPath;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.Arguments = Path.Combine(Environment.CurrentDirectory, SERVICE_EXE);
                p.Start();
                p.WaitForExit();
                return p.ExitCode == 0;
            }
        }

        public static bool StartService()
        {
            try
            {
                ServiceController service = ServiceController.GetServices().FirstOrDefault(x => x.ServiceName == SERVICE_NAME);
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(15));
                return true;
            } catch (System.ServiceProcess.TimeoutException)
            {
                return false;
            }
        }

        public static bool StopService()
        {
            try
            {
                ServiceController service = ServiceController.GetServices().FirstOrDefault(x => x.ServiceName == SERVICE_NAME);
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(15));
                return true;
            }
            catch (System.ServiceProcess.TimeoutException)
            {
                return false;
            }
        }
    }
}
