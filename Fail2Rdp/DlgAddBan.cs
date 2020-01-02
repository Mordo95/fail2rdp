using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fail2Rdp
{
    public partial class DlgAddBan : Form
    {
        public string Value => textBox1.Text;
        public DlgAddBan()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (IPAddress.TryParse(Value, out IPAddress address))
                DialogResult = DialogResult.OK;
            else
                MessageBox.Show("Address is not a valid IP address", "Validation failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
