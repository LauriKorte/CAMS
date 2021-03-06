﻿using System;
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
            sounds["winchester"] = "Sound/winchester_load.ogg";
            sounds["shotgun"] = "Sound/shotgun_shot.ogg";
            sounds["pickup"] = "Sound/pickup.ogg";
            sounds["knife"] = "Sound/swing.ogg";

            
            //Get all the map textures
            foreach (var v in maptex)
            {
                BitmapSource bms = (BitmapSource)Application.Current.Resources[v];
                textures[v] = bms;    
            }

            try
            {
                soundEngine = new ISoundEngine();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Couldn't initialize irrKlang: " + ex.Message);
            }
        }
        // Some music and sound stuff
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
            if (soundEngine == null)
                return;
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
            if (soundEngine == null)
                return;

            if (sounds.ContainsKey(soundName))
            {
                //Get the filename of the sound
                String snd = sounds[soundName];

                //Get the irrklang handle for the sound
                //(start it not looping and paused)
                ISound isnd = soundEngine.Play2D(snd, false, true);

                Random r = new Random();
                //Get a random float in the range of 0.85 - 1.15
                float pitch = ((float)(85 + r.Next(30))) / 100.0f;

                //Set the sound pitch
                isnd.PlaybackSpeed = pitch;

                //Set the sound volume
                //this can be used to alter sound volume
                isnd.Volume = 1.0f;

                //Play it
                isnd.Paused = false;
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
            if (Application.Current.Resources.Contains(t))
                return (BitmapSource)Application.Current.Resources[t];
            return null;
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
