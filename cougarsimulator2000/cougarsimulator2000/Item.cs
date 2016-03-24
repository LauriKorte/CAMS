using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        [XmlElement("weapon")]
        public List<WeaponDefinition> weapons { get; set; }
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

        protected ItemType type;
        public ItemType itemType
        {
            get
            {
                return type;
            }
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
        int damageDieSize
        {
            get;
            set;
        }
        [XmlElement("bonus")]
        int damageBonus
        {
            get;
            set;
        }
        WeaponDefinition()
        {
            type = ItemType.Weapon;
        }
    }

    public class Item
    {
        ItemDefinition definition;
        int count;
    }
}
