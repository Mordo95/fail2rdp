using Fail2Rdp.UI.Fail2RdpService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fail2Rdp.UI
{

    public enum SERVICE_STATUS
    {
        NOT_INSTALLED,
        NOT_STARTED,
        STARTED
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SERVICE_STATUS ServiceStatus { get; set; } = SERVICE_STATUS.NOT_INSTALLED;
        public Fail2RdpWCFServiceClient ServiceClient { get; set; } = null;

        private ListBox lstToRemove { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            RefreshData();
        }

        public void RefreshData()
        {
            ServiceController service = ServiceController.GetServices().FirstOrDefault(x => x.ServiceName == "Fail 2 RDP");
            if (service == null)
                ServiceStatus = SERVICE_STATUS.NOT_INSTALLED;
            else if (service.Status != ServiceControllerStatus.Running)
                ServiceStatus = SERVICE_STATUS.NOT_STARTED;
            else
                ServiceStatus = SERVICE_STATUS.STARTED;

            if (ServiceStatus == SERVICE_STATUS.STARTED && ServiceClient == null)
                ServiceClient = new Fail2RdpWCFServiceClient();
            else if (ServiceStatus != SERVICE_STATUS.STARTED)
                ServiceClient = null;

            UpdateUI();
        }

        public void UpdateUI()
        {
            lstBanned.Items.Clear();
            lstExcluded.Items.Clear();
            switch (ServiceStatus)
            {
                case SERVICE_STATUS.NOT_INSTALLED:
                    btnPlay.IsEnabled = true;
                    BtnStop.IsEnabled = false;
                    btnAddBan.IsEnabled = false;
                    btnAddExclusion.IsEnabled = false;
                    btnShiftToBanlist.IsEnabled = false;
                    btnShiftToExclusion.IsEnabled = false;
                    btnRemove.IsEnabled = false;
                    txtAttempts.IsEnabled = false;
                    lblStatus.Content = "Not Installed";
                    lblStatus.Foreground = Brushes.Red;
                    break;
                case SERVICE_STATUS.NOT_STARTED:
                    btnPlay.IsEnabled = true;
                    BtnStop.IsEnabled = false;
                    btnAddBan.IsEnabled = false;
                    btnAddExclusion.IsEnabled = false;
                    btnShiftToBanlist.IsEnabled = false;
                    btnShiftToExclusion.IsEnabled = false;
                    btnRemove.IsEnabled = false;
                    txtAttempts.IsEnabled = false;
                    lblStatus.Content = "Not Started";
                    lblStatus.Foreground = Brushes.Red;
                    break;
                case SERVICE_STATUS.STARTED:
                    btnPlay.IsEnabled = false;
                    BtnStop.IsEnabled = true;
                    btnAddBan.IsEnabled = true;
                    btnAddExclusion.IsEnabled = true;
                    btnShiftToBanlist.IsEnabled = false;
                    btnShiftToExclusion.IsEnabled = false;
                    btnRemove.IsEnabled = false;
                    txtAttempts.IsEnabled = true;
                    lblStatus.Content = "Running";
                    lblStatus.Foreground = Brushes.Green;
                    foreach (string entry in ServiceClient.GetBans())
                        lstBanned.Items.Add(entry);
                    foreach (string entry in ServiceClient.GetWhitelist())
                        lstExcluded.Items.Add(entry);
                    txtAttempts.Text = ServiceClient.GetThreshold().ToString();
                    break;
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Call service helper and start
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int attempts = 3;
            if (!string.IsNullOrEmpty(txtAttempts.Text))
                attempts = int.Parse(txtAttempts.Text);
            ServiceClient?.SetThreshold(attempts);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DlgAddAddress addressDialog = new DlgAddAddress();
            if (addressDialog.ShowDialog() == true && !string.IsNullOrEmpty(addressDialog.Value))
            {
                ServiceClient?.AddBan(addressDialog.Value);
                RefreshData();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DlgAddAddress addressDialog = new DlgAddAddress();
            if (addressDialog.ShowDialog() == true && !string.IsNullOrEmpty(addressDialog.Value))
            {
                ServiceClient?.AddWhitelist(addressDialog.Value);
                RefreshData();
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lstToRemove == null)
                return;
            foreach(string entry in new List<string>(lstToRemove.SelectedItems.Cast<string>()))
            {
                if (lstToRemove == lstBanned)
                    ServiceClient?.RemoveBan(entry);
                else
                    ServiceClient?.RemoveWhitelist(entry);
                lstToRemove.Items.Remove(entry);
            }
            btnRemove.IsEnabled = false;
        }

        private void lstIps_Focus(object sender, RoutedEventArgs e)
        {
            lstToRemove = e.Source as ListBox;
            btnRemove.IsEnabled = lstToRemove.SelectedItems.Count > 0;
        }

        private void lstExcluded_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*btnShiftToBanlist.IsEnabled = true;
            btnShiftToExclusion.IsEnabled = true;
            lstToRemove = e.Source as ListBox;
            btnRemove.IsEnabled = lstToRemove.SelectedItems.Count > 0;*/
        }

        private void lstBanned_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnShiftToBanlist.IsEnabled = true;
            btnShiftToExclusion.IsEnabled = true;
            lstToRemove = e.Source as ListBox;
            btnRemove.IsEnabled = lstToRemove.SelectedItems.Count > 0;
        }

        private void btnShiftToExclusion_Click(object sender, RoutedEventArgs e)
        {
            foreach (string entry in new List<string>(lstBanned.SelectedItems.Cast<string>()))
            {
                ServiceClient?.RemoveBan(entry);
                ServiceClient?.AddWhitelist(entry);
            }
            RefreshData();
        }

        private void btnShiftToBanlist_Click(object sender, RoutedEventArgs e)
        {
            foreach (string entry in new List<string>(lstExcluded.SelectedItems.Cast<string>()))
            {
                ServiceClient?.RemoveWhitelist(entry);
                ServiceClient?.AddBan(entry);
            }
            RefreshData();
        }
    }
}
