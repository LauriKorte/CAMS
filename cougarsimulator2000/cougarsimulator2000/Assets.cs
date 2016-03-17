using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

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
            return (ImageSource)Application.Current.Resources[t];
        }
    }
}
