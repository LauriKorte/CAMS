﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace cougarsimulator2000
{
    public enum ItemType
    {
        Weapon,
        Consumable,
        Artifact,
        Other
    }

    [XmlRoot("items")]
    public class ItemList
    {
        public int totalItemWeight
        { get; set;}

        public int getTotalItemWeightForLevel(int level)
        {
            int wt = 0;
            foreach (var i in all)
            {
                if (i.minimumLevel <= level)
                    wt += i.weight;
            }
            return wt;
        }
        public ItemList()
        {
            weapons = new List<WeaponDefinition>();
            others = new List<ItemDefinition>();
            all = new List<ItemDefinition>();
        }

        public ItemDefinition getItemDefinition(string str)
        {
            foreach (var o in all)
            {
                if (o.name == str)
                    return o;
            }
            return null;
        }

        public void loadItemImages(Assets ass)
        {
            foreach (var o in all)
                o.image = ass.getTextureImageSource(o.icon);
        }

        public void setupWeapons()
        {
            foreach(var w in weapons)
            {
                w.ammunitionDefinition = getItemDefinition(w.ammunitionType);
            }
        }

        [XmlElement("weapon")]
        public List<WeaponDefinition> weapons { get; set; }

        [XmlElement("other")]
        public List<ItemDefinition> others { get; set; }

        [XmlIgnore]
        public List<ItemDefinition> all { get; set; }

        public void combine()
        {
            foreach (var i in weapons)
                all.Add(i);
            foreach (var i in others)
                all.Add(i);
            totalItemWeight = 0;

            foreach (var i in all)
                totalItemWeight += i.weight;
        }
    }

    public class ItemDefinition
    {
        [XmlElement("name")]
        public string name
        {
            get;
            set;
        }

        [XmlElement("description")]
        public string description
        {
            get;
            set;
        }


        [XmlElement("icon")]
        public string icon { get; set; }


        [XmlIgnore]
        public ImageSource image
        {
            get;
            set;
        }

        //Not literal weight, but rather a random weight
        //used for loot spawning, greater means more abundant
        [XmlElement("weight")]
        public int weight { get; set; }

        //Determines the minimum level, the item may be spawned as loot
        [XmlElement("minimumLevel")]
        public int minimumLevel { get; set; }


        //The amount item will be spawned in one loot drop
        [XmlElement("spawnCount")]
        public int spawnCount { get; set; }

        //Maximum variation in both directions to the above spawnCount
        [XmlElement("spawnCountVariation")]
        public int spawnCountVariation { get; set; }

        protected ItemType type;
        public ItemType itemType
        {
            get
            {
                return type;
            }
        }

        public ItemDefinition()
        {
            type = ItemType.Other;
        }
    }

    public class WeaponDefinition : ItemDefinition
    {

        [XmlElement("dieCount")]
        public int damageDieCount
        {
            get;
            set;
        }

        [XmlElement("dieSize")]
        public int damageDieSize
        {
            get;
            set;
        }
        [XmlElement("bonus")]
        public int damageBonus
        {
            get;
            set;
        }

        [XmlElement("accuracy")]
        public int accuracy
        {
            get;
            set;
        }

        [XmlElement("fireSpeed")]
        public int fireSpeed
        {
            get;
            set;
        }

        [XmlElement("ammunition")]
        public string ammunitionType
        {
            get;
            set;
        }

        [XmlElement("fireSound")]
        public string fireSound
        {
            get;
            set;
        }

        [XmlIgnore]
        public ItemDefinition ammunitionDefinition
        {
            get;
            set;
        }

        public WeaponDefinition()
        {
            type = ItemType.Weapon;
        }
    }

    public class Item : INotifyPropertyChanged
    {
        public ItemDefinition definition
        {
            get;
        }
      

        public Item(ItemDefinition def, int cnt = 1)
        {
            definition = def;
            count = cnt;
        }

        private int _count;
        public int count
        {
            get { return _count; }
            set { _count = value; NotifyPropertyChanged("count"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
