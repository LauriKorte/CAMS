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
        private Assets assets;
        
        private List<Image> tileMapImages = new List<Image>();
        private List<Image> actorImages = new List<Image>();
        

        private void updateTileMap()
        {
            //Tells the size of a singular cell in our tilemap
            int gridCellW = 32;
            int gridCellH = 32;

            //Remove all the images which 
            TileMap tileMap = gameLogic.tileMap;

            gridTileMap.Children.Clear();

            tileMapImages = new List<Image>();

            int tileMapWidth = tileMap.size.x;
            int tileMapHeight = tileMap.size.y;

            SizeToContent = SizeToContent.WidthAndHeight;

            //If our tilemap has been resized...
            if (gridTileMap.ColumnDefinitions.Count != tileMapWidth
                || gridTileMap.RowDefinitions.Count != tileMapHeight)
            {
                //We just clear everything out

                //Clear all grid column and row definitions
                gridTileMap.ColumnDefinitions.Clear();
                gridTileMap.RowDefinitions.Clear();

                gridActors.ColumnDefinitions.Clear();
                gridActors.RowDefinitions.Clear();


                //Fill them with new column and row defintions
                for (int i = 0; i < tileMapWidth; i++)
                {
                    ColumnDefinition ra = new ColumnDefinition();
                    ra.Width = new GridLength(gridCellW);
                    gridTileMap.ColumnDefinitions.Add(ra);

                    ra = new ColumnDefinition();
                    ra.Width = new GridLength(gridCellW);
                    gridActors.ColumnDefinitions.Add(ra);
                }
                for (int j = 0; j < tileMapHeight; j++)
                {
                    RowDefinition rt = new RowDefinition();
                    rt.Height = new GridLength(gridCellH);
                    gridTileMap.RowDefinitions.Add(rt);

                    rt = new RowDefinition();
                    rt.Height = new GridLength(gridCellH);
                    gridActors.RowDefinitions.Add(rt);
                }
            }

            //Hard coded darkness image
            ImageSource darkened = assets.getTextureImageSource("bg_dithering");

            //Fill the grid with tilemap data
            for (int i = 0; i < tileMapWidth; i++)
            {

                for (int j = 0; j < tileMapHeight; j++)
                {
                    Grid imContainer = new Grid();

                    Tile t = tileMap.getTile(new Vector2(i, j));
                    if (t.isVisible == false)
                    {
                        Image dkim = new Image();
                        dkim.Source = darkened;
                        imContainer.Children.Add(dkim);
                        Grid.SetZIndex(dkim, 0);
                    }

                    //Every cell in the tilemap/grid has a single image
                    Image im = new Image();
                    im.Source = assets.getTileImageSource(t);


                    imContainer.Children.Add(im);
                    Grid.SetZIndex(im, -1);

                    gridTileMap.Children.Add(imContainer);
                    Grid.SetColumn(imContainer, i);
                    Grid.SetRow(imContainer, j);                  
                }
            }
        }

        private void updateActors()
        {
            //Remove the actor images
            gridActors.Children.Clear();

            actorImages = new List<Image>();

            //Get all of the game actors
            List<Actor> actors = gameLogic.actors;
            foreach (var a in actors)
            {
                if (!a.isVisible)
                    continue;
                //Create images for them
                Image im = new Image();
                gridActors.Children.Add(im);
                //Load correct image
                im.Source = assets.getTextureImageSource(a.image);

                //In case something is wrong in the game side
                //we don't want crashes
                if (a.position.x >= 0)
                    Grid.SetColumn(im, a.position.x);

                if (a.position.y >= 0)
                    Grid.SetRow(im, a.position.y);

                //Set the "depth"
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
            assets = ass;
            gameLogic = gl;

            updateActors();
            updateTileMap();
        }
    }
}
