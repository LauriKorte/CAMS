﻿using System;
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
        Settings settings;
        Assets assets;
        bool gameIsRunning = false;

        // create mainwindow
        public MainWindow()
        {
            InitializeComponent();
            assets = new Assets();

            gameLogic = new GameLogic(assets);
        }

        // start the game
        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            if (gameIsRunning == false)
            {
                constructGameView();
                constructControls();
                gameIsRunning = true;
            }
            else if (gameView.IsLoaded == true || controls.IsLoaded == true)
            {
                string messageboxTitle = "Achtung!";
                string messageboxAlert = "This will start a new game. Are you sure you want to do this?";
                MessageBoxButton btn = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Warning;

                // show the messagebox
                MessageBoxResult answer = MessageBox.Show(messageboxAlert, messageboxTitle, btn, icon);

                if (answer == MessageBoxResult.Yes)
                {
                    gameLogic.start();
                    gameView.update();
                }
                else
                {
                    // DO NOTHING
                }

            }

        }

        // create gameview
        private void constructGameView()
        {
            gameView = new GameView(assets, gameLogic);
            gameView.Show();
            gameView.Closing += GameView_Closing;
            menuchkShowGameView.IsChecked = true;
        }

        // create controls
        private void constructControls()
        {
            controls = new Controls(this);
            controls.Show();
            controls.Closing += Controls_Closing;
            menuchkShowControls.IsChecked = true;
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

/********************************************************************************************/

            // In this section there are some errors which shall be sorted out later
        private void menuchkShowControls_Checked(object sender, RoutedEventArgs e)
        {
            if (controls.IsLoaded)
            {
                controls.Show();
            }
            else
            {
                constructControls();
            }
        }

        private void menuchkShowGameView_Checked(object sender, RoutedEventArgs e)
        {
            if (gameView.IsLoaded)
            {
                gameView.Show();
            }
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
/*******************************************************************************************/

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // create messagebox
            string messageboxTitle = "Achtung!";
            string messageboxAlert = "Are you sure you want to quit?";
            MessageBoxButton btn = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Stop;

            // show the messagebox
            MessageBoxResult answer = MessageBox.Show(messageboxAlert, messageboxTitle, btn, icon);

            if (answer == MessageBoxResult.Yes)
            {
                gameView.Close();
                controls.Close();
            }
            else
            {
                e.Cancel = true; // some lifehacks
            }
          
        }

        private void btnDebugReroll_Click(object sender, RoutedEventArgs e)
        {
            gameLogic.start();
            gameView.update();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            settings = new Settings();
            settings.Show();
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
