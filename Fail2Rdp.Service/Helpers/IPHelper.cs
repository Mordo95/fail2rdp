using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fail2Rdp.Service.Helpers
{
    public static class IPHelper
    {
        public static bool IsValidAddress(string ip)
            => ip != null && ip != "-" && ip != "127.0.0.1" && IPAddress.TryParse(ip, out IPAddress address);
    }
}
