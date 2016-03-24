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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameLogic gameLogic;
        GameView gameView;
        Controls controls;
        Assets assets;


        private void constructGameView()
        {
            gameView = new GameView(assets, gameLogic);
            gameView.Show();
            gameView.Closing += GameView_Closing;
            menuchkShowGameView.IsChecked = true;
        }

        private void constructControls()
        {
            controls = new Controls(this);
            controls.Show();
            controls.Closing += Controls_Closing;
            menuchkShowControls.IsChecked = true;
        }

        public MainWindow()
        {
            InitializeComponent();
            assets = new Assets();

            gameLogic = new GameLogic(assets);

            constructGameView();
            constructControls();

        }

        private void Controls_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            menuchkShowControls.IsChecked = false;
        }

        private void GameView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            menuchkShowGameView.IsChecked = false;
        }

        public void passInput(Input i)
        {
            gameLogic.enterInput(i);
            gameView.update();
        }

        private void menuchkShowControls_Checked(object sender, RoutedEventArgs e)
        {
            if (controls.IsLoaded)
                controls.Show();
            else
            {
                constructControls();
            }
        }

        private void menuchkShowGameView_Checked(object sender, RoutedEventArgs e)
        {
            if (gameView.IsLoaded)
                gameView.Show();
            else
            {
                constructGameView();
            }
        }

        private void menuchkShowGameView_Unchecked(object sender, RoutedEventArgs e)
        {
            gameView.Hide();

        }

        private void menuchkShowControls_Unchecked(object sender, RoutedEventArgs e)
        {
            controls.Hide();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            if (gameView.IsLoaded)
                gameView.Close();
            if (controls.IsLoaded)
                controls.Close();
            
        }

        private void btnDebugReroll_Click(object sender, RoutedEventArgs e)
        {
            gameLogic.start();
            gameView.update();
        }
    }
}
