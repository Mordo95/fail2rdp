using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fail2Rdp.UI.Helpers
{
    public static class IPHelper
    {
        public static bool IsValidAddress(string ip)
            => !string.IsNullOrEmpty(ip) && ip != "-" && ip != "127.0.0.1" && ip != "0.0.0.0" && IPAddress.TryParse(ip, out _);
    }
}
