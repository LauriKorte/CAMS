﻿using System;
using System.Collections.Generic;
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
        public ItemList()
        {
            weapons = new List<WeaponDefinition>();
            others = new List<ItemDefinition>();
        }

        public ItemDefinition getItemDefinition(string str)
        {
            foreach (var w in weapons)
            {
                if (w.name == str)
                    return w;
            }
            foreach (var o in others)
            {
                if (o.name == str)
                    return o;
            }
            return null;
        }

        public void loadItemImages(Assets ass)
        {
            foreach (var w in weapons)
                w.image = ass.getTextureImageSource(w.icon);
            foreach (var o in others)
                o.image = ass.getTextureImageSource(o.icon);

        }

        [XmlElement("weapon")]
        public List<WeaponDefinition> weapons { get; set; }

        [XmlElement("other")]
        public List<ItemDefinition> others { get; set; }
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
        public ImageSource image;

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

        [XmlElement("ammunition")]
        public string ammunitionType
        {
            get;
            set;
        }
        public WeaponDefinition()
        {
            type = ItemType.Weapon;
        }
    }

    public class Item
    {
        public ItemDefinition definition;
        public int count;

        public Item(ItemDefinition def, int cnt = 1)
        {
            definition = def;
            count = cnt;
        }

    }
}
