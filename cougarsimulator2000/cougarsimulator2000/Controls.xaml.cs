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
    /// Interaction logic for Controls.xaml
    /// </summary>
    public partial class Controls : Window
    {
        public Inventory inventory;

        MainWindow main;
        public Controls(MainWindow mw)
        {
            main = mw;
            inventory = new Inventory();
            InitializeComponent();
        }

        private void Button_Click_N(object sender, RoutedEventArgs e)
        {
            main.passInput(Input.North);
        }

        private void Button_Click_W(object sender, RoutedEventArgs e)
        {
            main.passInput(Input.West);
        }

        private void Button_Click_S(object sender, RoutedEventArgs e)
        {
            main.passInput(Input.South);
        }

        private void Button_Click_E(object sender, RoutedEventArgs e)
        {
            main.passInput(Input.East);
        }

        private void Button_Click_NE(object sender, RoutedEventArgs e)
        {
            main.passInput(Input.NorthEast);
        }

        private void Button_Click_NW(object sender, RoutedEventArgs e)
        {
            main.passInput(Input.NorthWest);
        }

        private void Button_Click_SE(object sender, RoutedEventArgs e)
        {
            main.passInput(Input.SouthEast);
        }

        private void Button_Click_SW(object sender, RoutedEventArgs e)
        {
            main.passInput(Input.SouthWest);
        }

        private void Button_Click_star(object sender, RoutedEventArgs e)
        {
            if (inventory.IsLoaded == false)
            {
                Inventory inv2 = new Inventory();
                inv2.lbWeapons.ItemsSource = inventory.lbWeapons.ItemsSource;
                inventory = inv2;
            }
            inventory.Show();
        }
    }
}
