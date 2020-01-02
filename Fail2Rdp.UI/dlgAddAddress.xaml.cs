using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Fail2Rdp.UI
{
    /// <summary>
    /// Interaction logic for dlgAddAddress.xaml
    /// </summary>
    public partial class DlgAddAddress : Window
    {
        public string Value => txtValue.Text;
        public DlgAddAddress()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
