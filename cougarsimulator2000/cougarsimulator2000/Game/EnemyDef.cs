using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace cougarsimulator2000
{
    [XmlRoot("enemies")]
    public class EnemyList
    {

        public int getTotalItemWeightForLevel(int level)
        {
            int wt = 0;
            foreach (var i in enemies)
            {
                if (i.minimumLevel <= level)
                    wt += i.weight;
            }
            return wt;
        }

        public EnemyList()
        {
            enemies = new List<EnemyDefinition>();
        }

        [XmlElement("enemy")]
        public List<EnemyDefinition> enemies { get; set; }
    }

    public class EnemyDefinition
    {
        [XmlElement("name")]
        public string name { get; set; }

        [XmlElement("nameArticle")]
        public string nameArticle { get; set; }

        [XmlElement("nameDefArticle")]
        public string nameDefArticle { get; set; }

        [XmlElement("attackMessage")]
        public string attackMessage { get; set; }
        [XmlElement("attackDodgeMessage")]
        public string attackDodgeMessage { get; set; }
        [XmlElement("postMortem")]
        public string postMortem { get; set; }
        [XmlElement("goryPostMortem")]
        public string goryPostMortem { get; set; }

        [XmlElement("damageDieCount")]
        public int damageDieCount { get; set; }
        [XmlElement("damageDieSize")]
        public int damageDieSize { get; set; }
        [XmlElement("damageBonus")]
        public int damageBonus { get; set; }
        [XmlElement("attackAccuracy")]
        public int attackAccuracy { get; set; }
        [XmlElement("attackSpeed")]
        public int attackSpeed { get; set; }
        [XmlElement("moveSpeed")]
        public int moveSpeed { get; set; }
        [XmlElement("dodge")]
        public int dodge { get; set; }
        [XmlElement("health")]
        public int health { get; set; }
        [XmlElement("killScore")]
        public int killScore { get; set; }

        
        [XmlElement("weight")]
        public int weight { get; set; }
        
        [XmlElement("minimumLevel")]
        public int minimumLevel { get; set; }
        
        [XmlElement("danger")]
        public int danger { get; set; }

        [XmlElement("description")]
        public string description
        {
            get;
            set;
        }

        [XmlElement("image")]
        public string image { get; set; }


    }
}
