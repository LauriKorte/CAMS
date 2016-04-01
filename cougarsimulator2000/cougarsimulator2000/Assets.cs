using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;

namespace cougarsimulator2000
{

    public class Assets
    {
        private Dictionary<string, BitmapSource> textures = new Dictionary<string, BitmapSource>();

        public Assets()
        {
            string[] maptex = new string[]
            {
                "bg_sand","bg_wall"
            };

            string dithered = "bg_dithered";
            BitmapSource ims = (BitmapSource)Application.Current.Resources[dithered];

            foreach (var v in maptex)
            {
                BitmapSource bms = (BitmapSource)Application.Current.Resources[v];
                textures[v] = bms;
                
            }
        }

        public ImageSource getTileImageSource(Tile t)
        {
            if (t.type != 0)
                return textures["bg_sand"];
            return textures["bg_wall"];
        }
        public ImageSource getTextureImageSource(string t)
        {
            if (t == null)
                return null;
            return (BitmapSource)Application.Current.Resources[t];
        }

        public ItemList loadItems()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(ItemList));
                using (var xmlReader = XmlReader.Create(Application.GetContentStream(new Uri("content/items.xml", UriKind.Relative)).Stream))
                {
                    var items = (ItemList)ser.Deserialize(xmlReader);
                    items.loadItemImages(this);
                    items.setupWeapons();
                    return items;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return new ItemList();
        }

        public GameDefinitions loadGameDefinitions()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(GameDefinitions));
                using (var xmlReader = XmlReader.Create(Application.GetContentStream(new Uri("content/game.xml", UriKind.Relative)).Stream))
                {
                    var defs = (GameDefinitions)ser.Deserialize(xmlReader);
                    return defs;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return new GameDefinitions();
        }
    }
}
