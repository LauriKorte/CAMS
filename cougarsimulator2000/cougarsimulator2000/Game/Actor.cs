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
        public Vector2 position;
        private int _health;
        public int health
        {
            get { return _health; }

            set { _health = value; NotifyPropertyChanged("health"); }
        }

        public int depth = 0;
        public bool isDead = false;
        public string image;

        public string nameArticle = "a ";
        public string nameDefArticle = "the ";

        private string _name;
        public string name
        {
            get { return _name; }
            
            set { _name = value; NotifyPropertyChanged("name"); }
        }

        public string postMortem = "da ding is dead";

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
            name = "nameless";
        }

        virtual public bool attack(GameLogic gl, Actor ac)
        {
            return false;
        }

        virtual public void update(GameLogic gl)
        {

        }

        virtual public void damage(GameLogic gl, int dam, string message)
        {
            gl.logGameMessage(nameDefArticle, name, " ", message, " for ", dam, " damage");
            health -= dam;
            if (health <= 0)
            {
                isDead = true;
                gl.logGameMessage(postMortem);
                gl.removeActor(this);
            }
        }
    }
}
