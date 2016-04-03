using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cougarsimulator2000
{
    public class Actor : INotifyPropertyChanged
    {
        public bool isRemoved = false;
        public int moveSpeed = 10;
        public Vector2 position;
        private int _health;
        private int _dodge;
        public bool isVisible = true;

        public int dodge
        {
            get { return _dodge; } 

            set { _dodge = value; NotifyPropertyChanged("dodge"); }
        }

        public int health
        {
            get { return _health; }

            set { _health = value; NotifyPropertyChanged("health"); }
        }

        public int depth = 0;
        public bool isDead = false;
        public string image;

        public string nameArticle = "a ";
        public string nameDefArticle = "The ";

        private string _name;
        public string name
        {
            get { return _name; }
            
            set { _name = value; NotifyPropertyChanged("name"); }
        }

        public string postMortem = "da ding is dead";
        public string goryPostMortem = "da ding is really dead";

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool isBlocking
        {
            get;
            set;
        }
        public bool isEnemy
        {
            get;
            set;
        }
        public Actor()
        {
            isBlocking = true;
            isEnemy = false;
            health = 10;
            dodge = 0;
            name = "nameless";
        }

        virtual public int attack(GameLogic gl, Actor ac)
        {
            return 0;
        }
        
        virtual public int update(GameLogic gl)
        {
            return 1999;
        }

        virtual public void damage(GameLogic gl, Attack ak)
        {
            Random r = new Random();
            int dodgeDice = 1;
            dodgeDice += r.Next(6);
            dodgeDice += r.Next(6);
            dodgeDice += r.Next(6);

            int accuracy = ak.accuracy - dodge;
            if (dodgeDice > accuracy)
            {
                gl.logGameMessage(nameDefArticle, name, " ", ak.dodgeMessage);
                return;
            }
            gl.logGameMessage(nameDefArticle, name, " ", ak.damageMessage, " for ", ak.damage, " damage");
            health -= ak.damage;
            if (health <= 0)
            {
                isDead = true;
                if (health <= -10)
                    gl.logGameMessage(goryPostMortem);
                else
                    gl.logGameMessage(postMortem);
                gl.removeActor(this);
            }
        }
    }
}
