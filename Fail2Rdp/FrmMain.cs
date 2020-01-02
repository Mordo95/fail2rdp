using Fail2Rdp.helpers;
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
            try
            {
                if (ServiceHelper.UninstallService())
                    MessageBox.Show("Success", "Deinstallation complete", MessageBoxButtons.OK);
                else
                    MessageBox.Show("Cannot deinstall. Please check execution privileges.", "Uninstallation error", MessageBoxButtons.OK);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Deinstallation error", MessageBoxButtons.OK);
            }
            finally
            {
                RefreshData();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (ServiceHelper.InstallService())
                    MessageBox.Show("Success", "Installation complete", MessageBoxButtons.OK);
                else
                    MessageBox.Show("Cannot install. Please check execution privileges.", "Installation error", MessageBoxButtons.OK);

            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Installation error", MessageBoxButtons.OK);
            } finally
            {
                RefreshData();
            }
        }
    }
}
