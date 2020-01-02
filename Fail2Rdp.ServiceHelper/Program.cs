using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fail2Rdp.ServiceHelper
{
    class Program
    {
        static int Main(string[] args)
        {
            bool success = false;

            foreach(string arg in args)
            {
                switch (arg)
                {
                    case "-i":
                    case "--install":
                        Console.WriteLine($"[+] Installing service...");
                        success |= ServiceHelper.InstallService();
                        Console.WriteLine($"[{(success ? "+" : "-")}] Service installation {(success ? "succeeded" : "failed")}");
                        break;
                    case "-u":
                    case "--uninstall":
                        Console.WriteLine($"[+] Removing service...");
                        success |= ServiceHelper.InstallService();
                        Console.WriteLine($"[{(success ? "+" : "-")}] Service removal {(success ? "succeeded" : "failed")}");
                        break;
                    case "-r":
                    case "--remove":
                        Console.WriteLine($"[+] Restarting service...");
                        success |= ServiceHelper.StopService();
                        Console.WriteLine($"[{(success ? "+" : "-")}] Stopping service {(success ? "succeeded" : "failed")}");
                        success |= ServiceHelper.StartService();
                        Console.WriteLine($"[{(success ? "+" : "-")}] Starting service {(success ? "succeeded" : "failed")}");
                        break;
                    case "-s":
                    case "--start":
                        Console.WriteLine($"[+] Starting service...");
                        success |= ServiceHelper.StartService();
                        Console.WriteLine($"[{(success ? "+" : "-")}] Starting service {(success ? "succeeded" : "failed")}");
                        break;
                    case "-S":
                    case "--stop":
                        Console.WriteLine($"[+] Stopping service...");
                        success |= ServiceHelper.StopService();
                        Console.WriteLine($"[{(success ? "+" : "-")}] Stopping service {(success ? "succeeded" : "failed")}");
                        break;
                }
            }

            return success ? 0 : -1;
        }
    }
}
