using Fail2Rdp.Service.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Fail2Rdp.Service
{
    public partial class LogReadingService : ServiceBase
    {
        EventLog LoginLogSubscription = null;
        public ServiceHost WCFServiceHost = null;
        public Dictionary<string, int> Attempts = null;
        Fail2RdpWCFService Logic = null;

        public LogReadingService()
        {
            InitializeComponent();
        }

        private void OnEntryWritten(object sender, EntryWrittenEventArgs e)
        {
            if (e.Entry.InstanceId == 4625) // logon
            {
                EventXml xml = ParseRecord(e.Entry);
                var ipAddress = xml.EventData["IpAddress"];
                var loginType = xml.EventData["LogonType"];
                if (IPHelper.IsValidAddress(ipAddress) && loginType == "3")
                {
                    if (Attempts.ContainsKey(ipAddress))
                    {
                        Attempts[ipAddress]++;
                        if (Attempts[ipAddress] > Program.Settings.Threshold && !Logic.GetBans().Contains(ipAddress))
                            Logic.AddBan(ipAddress);
                    }
                    else
                        Attempts[ipAddress] = 1;
                }
            }
        }

        static EventXml ParseRecord(EventLogEntry entry)
        {
            using (EventLogReader reader = new EventLogReader(new EventLogQuery("Security", PathType.LogName, "*[System[(EventRecordID=" + entry.Index + ")]]")))
            {
                EventLogRecord record = reader.ReadEvent() as EventLogRecord;
                return EventXml.Parse(record.ToXml());
            }
        }

        protected override void OnStart(string[] args)
        {
            // Settings
            Program.Settings = Settings.Load();
            Attempts = new Dictionary<string, int>();
            Logic = new Fail2RdpWCFService();
            // Log subscription
            LoginLogSubscription = new EventLog
            {
                Log = "Security"
            };
            LoginLogSubscription.EntryWritten += new EntryWrittenEventHandler(OnEntryWritten);
            LoginLogSubscription.EnableRaisingEvents = true;

            // WCF
            if (WCFServiceHost != null)
            {
                WCFServiceHost.Close();
            }
            WCFServiceHost = new ServiceHost(typeof(Fail2RdpWCFService));

            WCFServiceHost.Open();
        }

        protected override void OnStop()
        {
            if (Program.Settings != null)
            {
                Program.Settings.Save();
                Program.Settings = null;
            }

            if (LoginLogSubscription != null)
            {
                LoginLogSubscription.Dispose();
                LoginLogSubscription = null;
            }
            if (Attempts != null)
                Attempts = null;
            if (Logic != null)
                Logic = null;

            if (WCFServiceHost != null)
            {
                WCFServiceHost.Close();
                WCFServiceHost = null;
            }
        }
    }
}
