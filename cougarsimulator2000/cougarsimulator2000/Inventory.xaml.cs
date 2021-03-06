﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
        MainWindow mainWindow;
        public Inventory(MainWindow mw)
        {
            mainWindow = mw;
            InitializeComponent();
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnEquip_Click(object sender, RoutedEventArgs e)
        {
            Item selitem = lbWeapons.SelectedItem as Item;
            if (selitem != null)
            {
                mainWindow.passEquip(selitem);     
            }
        }
    }
}
