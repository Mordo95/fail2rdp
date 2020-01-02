using Fail2Rdp.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Fail2Rdp.Service
{
    [ServiceContract(Namespace = "http://Fail2Rdp.Service")]
    public interface IFail2RdpWCFService
    {
        [OperationContract]
        void AddBan(string ip);
        [OperationContract]
        bool RemoveBan(string ip);
        [OperationContract]
        List<string> GetBans();
        [OperationContract]
        int GetThreshold();
        [OperationContract]
        void SetThreshold(int threshold);
        /*[OperationContract]
        void AddPermaBan(string ip);
        [OperationContract]
        void RemovePermaBan(string ip);
        [OperationContract]
        void AddWhitelist(string ip);
        [OperationContract]
        void RemoveWhitelist(string ip);*/
    }

    public class Fail2RdpWCFService : IFail2RdpWCFService
    {
        public void AddBan(string ip)
        {
            if (!IPHelper.IsValidAddress(ip))
                return;
            Program.Settings.Bans.Add(ip);
            FirewallHelper.AddFirewallRule(ip);
            Program.Settings.Save();
        }

        public List<string> GetBans()
        {
            return Program.Settings.Bans;
        }

        public int GetThreshold()
        {
            return Program.Settings.Threshold;
        }

        public bool RemoveBan(string ip)
        {
            if (!IPHelper.IsValidAddress(ip))
                return false;
            bool success = Program.Settings.Bans.Remove(ip);
            FirewallHelper.RemoveFirewallRule(ip);
            Program.Settings.Save();
            return success;
        }

        public void SetThreshold(int threshold)
        {
            Program.Settings.Threshold = threshold;
            Program.Settings.Save();
        }
    }
}
