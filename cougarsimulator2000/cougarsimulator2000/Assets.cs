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
using IrrKlang;

namespace cougarsimulator2000
{

    public class Assets
    {
        private ISoundEngine soundEngine;
        private ISound playingMusic;
        private Dictionary<String, BitmapSource> textures = new Dictionary<String, BitmapSource>();
        private Dictionary<String, String> musics = new Dictionary<String, String>();
        private Dictionary<String, String> sounds = new Dictionary<String, String>();

        public Assets()
        {
            String[] maptex = new String[]
            {
                "bg_sand","bg_wall"
            };

            // Make the music come alive
            musics["main"] = "Sound/hyperfun.ogg";

            sounds["peacemaker"] = "Sound/peacemaker.ogg";
            sounds["m1860"] = "Sound/crappygun.ogg";
            sounds["winchester"] = "Sound/winchester.ogg";

            String dithered = "bg_dithered";
            BitmapSource ims = (BitmapSource)Application.Current.Resources[dithered];

            foreach (var v in maptex)
            {
                BitmapSource bms = (BitmapSource)Application.Current.Resources[v];
                textures[v] = bms;
                
            }
            soundEngine = new ISoundEngine();
        }

        public void setMusicVolume(double volume)
        {
            double vol = volume;
            if (playingMusic != null)
            {
                playingMusic.Volume = (float)vol;
            }
        }

        public void playMusic(string musicName)
        {
            if (playingMusic != null)
            {
                playingMusic.Stop();
                playingMusic = null;
            }
            if (musics.ContainsKey(musicName))
            {
                String music = musics[musicName];
                playingMusic = soundEngine.Play2D(music, true);
            }
        }

        public void playSound(string soundName)
        {
            if (sounds.ContainsKey(soundName))
            {
                String snd = sounds[soundName];
                soundEngine.Play2D(snd, false);
            }
        }
        public ImageSource getTileImageSource(Tile t)
        {
            if (t.type != 0)
                return textures["bg_sand"];
            return textures["bg_wall"];
        }
        public ImageSource getTextureImageSource(String t)
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
                    items.combine();
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


        public EnemyList loadEnemies()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(EnemyList));
                using (var xmlReader = XmlReader.Create(Application.GetContentStream(new Uri("content/enemies.xml", UriKind.Relative)).Stream))
                {
                    var items = (EnemyList)ser.Deserialize(xmlReader);
                    return items;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return new EnemyList();
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
