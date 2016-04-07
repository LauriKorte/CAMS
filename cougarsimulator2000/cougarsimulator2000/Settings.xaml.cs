using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        Assets assets;

        public Settings(Assets ass)
        {
            assets = ass;
            InitializeComponent();
        }

        private void btnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            // SAVE THE SETTINGS
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void sldrMusicVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Possible game music update will be added here
            assets.setMusicVolume(sldrMusicVolume.Value);
        }

        private void sldrGameVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Possible game sound update will be added here
        }

        private void btnHighScore_Click(object sender, RoutedEventArgs e)
        {
            (new Highscore()).ShowDialog();
        }
    }
}
