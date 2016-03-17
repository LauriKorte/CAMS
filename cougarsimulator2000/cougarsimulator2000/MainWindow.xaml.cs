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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace cougarsimulator2000
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<List<Image>> tileMapData { get; set; }


        public int cougarPosX = 4;
        public int cougarPosY = 6;
        int tileMapWidth = 16;
        int tileMapHeight = 16;
        Image cougar;
        Controls controls;

        public void UpdateCougar()
        {
            if (cougarPosX < 0)
                cougarPosX = 0;
            if (cougarPosY < 0)
                cougarPosY = 0;
            if (cougarPosX >= tileMapWidth)
                cougarPosX = tileMapWidth-1;
            if (cougarPosY >= tileMapHeight)
                cougarPosY = tileMapHeight-1;
            Grid.SetRow(cougar, cougarPosY);
            Grid.SetColumn(cougar, cougarPosX);
        }
        public MainWindow()
        {
            controls = new Controls(this);
            controls.Show();
            InitializeComponent();




            for (int i = 0; i < tileMapWidth; i++)
            {
                ColumnDefinition ra = new ColumnDefinition();
                ra.Width = new GridLength(32);
                TileMap.ColumnDefinitions.Add(ra);
            }
            for (int j = 0; j < tileMapHeight; j++)
            {
                RowDefinition rt = new RowDefinition();
                rt.Height = new GridLength(32);
                TileMap.RowDefinitions.Add(rt);
            }
            
            ImageSource sc1 = (ImageSource)Resources["bg_grass1"];
            ImageSource sc2 = (ImageSource)Resources["bg_box"];

            tileMapData = new List<List<Image>>();
            Random r = new Random();
            for (int i = 0;  i < tileMapWidth; i++)
            {
                List<Image> l = new List<Image>();
                for (int j = 0; j < tileMapHeight; j++)
                {
                    Image im = new Image();
                    if (r.Next(100) < 50)
                        im.Source = sc1;
                    else
                        im.Source = sc2;
                    
                    l.Add(im);
                }
                tileMapData.Add(l);
            }

            for (int i = 0; i < tileMapWidth; i++)
                for (int j = 0; j < tileMapHeight; j++)
                {
                    Image l2 = tileMapData[i][j];
                    TileMap.Children.Add(l2);
                    Grid.SetRow(l2, j);
                    Grid.SetColumn(l2, i);

                }
            cougar =  new Image();
            cougar.Source = (ImageSource)Resources["ac_cougar"];
            TileMap.Children.Add(cougar);
            Grid.SetRow(cougar, cougarPosY);
            Grid.SetColumn(cougar, cougarPosX);

            cougar.MouseEnter += new MouseEventHandler((object sender, MouseEventArgs e) =>
            {
                Random r2 = new Random();
                cougarPosX += r.Next(3) - 1;
                cougarPosY += r.Next(3) - 1;

                Grid.SetRow(cougar, cougarPosY);
                UpdateCougar();
            });

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controls.Close();
        }
    }
}
