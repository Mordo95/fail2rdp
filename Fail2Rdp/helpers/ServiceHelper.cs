using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fail2Rdp.helpers
{
    public static class ServiceHelper
    {
        private const string SERVICE_EXE = "Fail2Rdp.Service.exe";


        public static bool UninstallService()
        {
            if (!File.Exists(SERVICE_EXE))
                throw new Exception("Service component is missing. Please reinstall.");
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
            /*if (p.ExitCode == 0)
            {
                MessageBox.Show("Success", "Uninstall complete", MessageBoxButtons.OK);
                RefreshData();
            }
            else
            {
                MessageBox.Show("Cannot uninstall. Please check execution privileges.", "Uninstallation error", MessageBoxButtons.OK);
            }*/
        }

        public static bool InstallService()
        {
            if (!File.Exists(SERVICE_EXE))
                throw new Exception("Service component is missing. Please reinstall.");
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
            /*if (p.ExitCode == 0)
            {
                MessageBox.Show("Success", "Installation complete", MessageBoxButtons.OK);
                RefreshData();
            }
            else
            {
                MessageBox.Show("Cannot install. Please check execution privileges.", "Installation error", MessageBoxButtons.OK);
            }*/
        }
    }
}
