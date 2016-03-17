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
    public partial class GameView : Window
    {
        private GameLogic gameLogic;
        private TileMap tileMap;
        private Assets assets;
        private List<Image> tileMapImages = new List<Image>();
        private List<Image> actorImages = new List<Image>();
        

        private void updateTileMap()
        {
            foreach (var im in tileMapImages)
                gridTileMap.Children.Remove(im);
            tileMapImages = new List<Image>();

            int tileMapWidth = tileMap.getSize().x;
            int tileMapHeight = tileMap.getSize().y;

            for (int i = 0; i < tileMapWidth; i++)
            {
                ColumnDefinition ra = new ColumnDefinition();
                ra.Width = new GridLength(32);
                gridTileMap.ColumnDefinitions.Add(ra);
            }
            for (int j = 0; j < tileMapHeight; j++)
            {
                RowDefinition rt = new RowDefinition();
                rt.Height = new GridLength(32);
                gridTileMap.RowDefinitions.Add(rt);
            }

            for (int i = 0; i < tileMapWidth; i++)
            {

                for (int j = 0; j < tileMapHeight; j++)
                {
                    Image im = new Image();
                    im.Source = assets.getTileImageSource(tileMap.getTile(new Vector2(i, j)));

                    gridTileMap.Children.Add(im);
                    Grid.SetRow(im, i);
                    Grid.SetColumn(im, j);
                    Grid.SetZIndex(im, -1);
                    tileMapImages.Add(im);
                }
            }
        }

        private void updateActors()
        {
            foreach (var im in actorImages)
                gridTileMap.Children.Remove(im);
            actorImages = new List<Image>();

            List<Actor> actors = gameLogic.actors;
            foreach (var a in actors)
            {
                Image im = new Image();
                gridTileMap.Children.Add(im);
                im.Source = assets.getTextureImageSource(a.image);
                if (a.position.x >= 0)
                    Grid.SetColumn(im, a.position.x);
                if (a.position.y >= 0)
                    Grid.SetRow(im, a.position.y);

                Grid.SetZIndex(im, a.depth);

                actorImages.Add(im);
            }
        }

        public void update()
        {
            updateActors();
            updateTileMap();
        }

        public GameView(Assets ass, GameLogic gl)
        {
            InitializeComponent();
            tileMap = gl.tileMap;
            assets = ass;
            gameLogic = gl;

            updateActors();
            updateTileMap();
        }
    }
}
