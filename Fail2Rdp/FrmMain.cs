using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fail2Rdp
{
    public partial class FrmMain : Form
    {
        Fail2RdpService.Fail2RdpWCFServiceClient client = null;
        public FrmMain()
        {
            InitializeComponent();
            RefreshData();
        }

        public void RefreshData()
        {
            ServiceController service = ServiceController.GetServices().FirstOrDefault(x => x.ServiceName == "Fail 2 RDP");
            if (service == null) {
                lblStatus.Text = "Not Installed";
                lblStatus.ForeColor = Color.Red;
            } else if (service.Status != ServiceControllerStatus.Running)
            {
                lblStatus.Text = "Not Running";
                lblStatus.ForeColor = Color.Red;
            } else
            {
                lblStatus.Text = "Running";
                lblStatus.ForeColor = Color.Green;
                client = new Fail2RdpService.Fail2RdpWCFServiceClient();
            }
            listBox1.Items.Clear();
            if (client != null)
            {
                listBox1.Items.AddRange(client.GetBans());
                textBox1.Text = client.GetThreshold().ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DlgAddBan addBanDialog = new DlgAddBan();
            if (addBanDialog.ShowDialog() == DialogResult.OK)
            {
                client.AddBan(addBanDialog.Value);
                RefreshData();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
                button2.Enabled = true;
            else
                button2.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach(var entry in listBox1.SelectedItems)
                client.RemoveBan(entry as string);
            RefreshData();
            button2.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                client.SetThreshold(int.Parse(textBox1.Text));
                RefreshData();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!File.Exists("Fail2Rdp.Service.exe"))
            {
                MessageBox.Show("Service component is missing. Please reinstall.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string dotNetPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            Process p = new Process();
            p.StartInfo.FileName = "InstallUtil.exe";
            p.StartInfo.WorkingDirectory = dotNetPath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.Arguments = "-u " + Path.Combine(Environment.CurrentDirectory, "Fail2Rdp.Service.exe");
            p.Start();
            p.WaitForExit();
            if (p.ExitCode == 0)
            {
                MessageBox.Show("Success", "Uninstall complete", MessageBoxButtons.OK);
                RefreshData();
            }
            else
            {
                MessageBox.Show("Cannot uninstall. Please check execution privileges.", "Uninstallation error", MessageBoxButtons.OK);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!File.Exists("Fail2Rdp.Service.exe"))
            {
                MessageBox.Show("Service component is missing. Please reinstall.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string dotNetPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            Process p = new Process();
            p.StartInfo.FileName = "InstallUtil.exe";
            p.StartInfo.WorkingDirectory = dotNetPath;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.Arguments = Path.Combine(Environment.CurrentDirectory, "Fail2Rdp.Service.exe");
            p.Start();
            p.WaitForExit();
            if (p.ExitCode == 0)
            {
                MessageBox.Show("Success", "Installation complete", MessageBoxButtons.OK);
                RefreshData();
            } else
            {
                MessageBox.Show("Cannot install. Please check execution privileges.", "Installation error", MessageBoxButtons.OK);
            }
        }
    }
}
