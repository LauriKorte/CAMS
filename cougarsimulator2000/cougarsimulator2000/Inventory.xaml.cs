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

namespace cougarsimulator2000
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class Inventory : Window
    {
        public Inventory()
        {
            InitializeComponent();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void M1860_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Colt Army Model 1860");
        }
        private void M1873_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Colt Model 1873 'Peacemaker'");
        }
        private void Winchester_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Winchester Model 1873");
        }
    }
}
