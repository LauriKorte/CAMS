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
        private MainWindow mainWindow;
        private Assets assets;

        //The ordering of these arrays are a bit important
        private List<Image> tileMapTiles = new List<Image>();
        private List<Image> tileMapVisibility = new List<Image>();

        private void updateTileMap()
        {
            //Tells the size of a singular cell in our tilemap
            int gridCellW = 32;
            int gridCellH = 32;

            TileMap tileMap = gameLogic.tileMap;

            int tileMapWidth = tileMap.size.x;
            int tileMapHeight = tileMap.size.y;

            SizeToContent = SizeToContent.WidthAndHeight;

            //Hard coded darkness image
            ImageSource darkened = assets.getTextureImageSource("bg_dithering");

            //If our tilemap has been resized...
            if (gridTileMap.ColumnDefinitions.Count != tileMapWidth
                || gridTileMap.RowDefinitions.Count != tileMapHeight)
            {
                //We just clear everything out and start over again

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

                gridTileMap.Children.Clear();
                tileMapTiles.Clear();
                tileMapVisibility.Clear();

                //Fill the grid with tilemap data
                for (int j = 0; j < tileMapHeight; j++)
                {
                    for (int i = 0; i < tileMapWidth; i++)
                    {
                        Grid imContainer = new Grid();

                        Tile t = tileMap.getTile(new Vector2(i, j));
                        //Create the overlaid darkness
                        Image dkim = new Image();
                        dkim.Source = darkened;
                        imContainer.Children.Add(dkim);
                        Grid.SetZIndex(dkim, 0);
                        if (t.isVisible == true)
                        {
                            //If we see the cell, hide the darkness
                            dkim.Visibility = Visibility.Hidden;
                        }

                        tileMapVisibility.Add(dkim);

                        //Every cell in the tilemap/grid has a single image
                        Image im = new Image();
                        im.Source = assets.getTileImageSource(t);
                        

                        imContainer.Children.Add(im);
                        Grid.SetZIndex(im, -1);

                        tileMapTiles.Add(im);

                        gridTileMap.Children.Add(imContainer);
                        Grid.SetColumn(imContainer, i);
                        Grid.SetRow(imContainer, j);

                        
                    }
                }
            }
            else
            {
                //If our tilemap is the same size,
                //we can just update the old images instead of doing an expensive
                //rebuilding of the map

                //It doesn't really matter for a turn based game
                
                foreach (Tile t in tileMap.tiles)
                {
                    tileMapTiles[t.tileIndex].Source = assets.getTileImageSource(t);
                    if (!t.isVisible)
                        tileMapVisibility[t.tileIndex].Visibility = Visibility.Visible;
                    else
                        tileMapVisibility[t.tileIndex].Visibility = Visibility.Hidden;
                }
            }
        }

        private void updateActors()
        {
            //Remove the actor images
            gridActors.Children.Clear();

            //Get all of the game actors
            List<Actor> actors = gameLogic.actors;
            foreach (var a in actors)
            {
                Grid gr = new Grid();
                if (!a.isVisible)
                    continue;
                //Create images for them
                Image im = new Image();
                gr.Children.Add(im);
                //Load correct image
                im.Source = assets.getTextureImageSource(a.image);
                im.ToolTip = a.nameArticle+a.name;

                gridActors.Children.Add(gr);

                //In case something is wrong in the game side
                //we don't want crashes
                if (a.position.x >= 0)
                    Grid.SetColumn(gr, a.position.x);

                if (a.position.y >= 0)
                    Grid.SetRow(gr, a.position.y);

                
                //Set the "depth"
                Grid.SetZIndex(gr, a.depth);
                im.Cursor = Cursors.Cross;
                im.MouseDown += (ignore1, ignore2)
                =>
                {
                    mainWindow.passInteraction(a);
                };
            }
        }

        public void update()
        {
            updateActors();
            updateTileMap();
        }

        public GameView(MainWindow mw, Assets ass, GameLogic gl)
        {
            InitializeComponent();
            mainWindow = mw;
            assets = ass;
            gameLogic = gl;

            updateActors();
            updateTileMap();
        }
    }
}
