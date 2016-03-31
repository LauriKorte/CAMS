using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;

namespace cougarsimulator2000
{
    public class Assets
    {
        public ImageSource getTileImageSource(Tile t)
        {
            if (t.type != 0)
                return (ImageSource)Application.Current.Resources["bg_grass1"];
            return (ImageSource)Application.Current.Resources["bg_box"];
        }
        public ImageSource getTextureImageSource(string t)
        {
            if (t == null)
                return null;
            return (ImageSource)Application.Current.Resources[t];
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
