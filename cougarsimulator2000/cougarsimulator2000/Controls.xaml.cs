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
        MainWindow main;
        public Controls(MainWindow mw)
        {
            main = mw;
            InitializeComponent();
        }

        private void Button_Click_N(object sender, RoutedEventArgs e)
        {
            main.cougarPosY--;
            main.UpdateCougar();
        }

        private void Button_Click_W(object sender, RoutedEventArgs e)
        {
            main.cougarPosX--;
            main.UpdateCougar();
        }

        private void Button_Click_S(object sender, RoutedEventArgs e)
        {
            main.cougarPosY++;
            main.UpdateCougar();
        }

        private void Button_Click_E(object sender, RoutedEventArgs e)
        {
            main.cougarPosX++;
            main.UpdateCougar();
        }
    }
}
