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
        public int armorRating = 0;
        public Vector2 position;
        private int _health;
        private int _dodge;
        public bool isVisible = true;
        public double imageAngle = 0.0;

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

        public int maxHealth
        {
            get;set;
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

        virtual public void onDeath(GameLogic gl)
        {

        }

        virtual protected int doAttack(GameLogic gl, Attack ak)
        {
            Random r = new Random();
            int dodgeDice = 1;
            dodgeDice += r.Next(6);
            dodgeDice += r.Next(6);
            dodgeDice += r.Next(6);

            int accuracy = ak.accuracy - dodge;
            if (dodgeDice > accuracy)
            {
                return -1;
            }

            //Each armor rating point has 33% chance to remove a single point of damage

            int armor = 0;
            for (int i = 0; i < armorRating; i++)
                if (r.Next(6) >= 4)
                    armor += 1;
            int dam = ak.damage;
            dam -= armor;
            if (dam <= 0)
                dam = 0;
            return dam;
        }

        virtual protected void spawnBlood(GameLogic gl)
        {

            Random r = new Random();
            if (r.Next(3) == 1)
                return;
            Prop p = new Prop("blood1");
            gl.addActor(p);
            p.position = position;
        }

        virtual protected void handleDamage(GameLogic gl, Attack ak, int dam)
        {
            if (dam == -1)
            {
                gl.logGameMessage(nameDefArticle, name, " ", ak.dodgeMessage);
                return;
            }

            if (dam > 0)
            {
                gl.logGameMessage(nameDefArticle, name, " ", ak.damageMessage, " for ", dam, " damage");
                health -= dam;
            }
            else
            {
                gl.logGameMessage(nameDefArticle, name, " ", ak.damageMessage, ", but remains unscathed");
                return;
            }
            spawnBlood(gl);
            if (health <= 0)
            {

                onDeath(gl);
                isDead = true;
                if (health <= -10)
                    gl.logGameMessage(goryPostMortem);
                else
                    gl.logGameMessage(postMortem);
                gl.removeActor(this);
            }
        }

        virtual public bool damageMultiple(GameLogic gl, List<Attack> ak)
        {
            if (isDead)
                return false;
            if (ak.Count == 0)
                return false;

            int dam = 0;
            int hitCount = 0;
            bool dodged = true;
            foreach (var atk in ak)
            {
                int tdam = doAttack(gl, atk);
                if (tdam >= 0)
                {
                    dam += tdam;
                    hitCount++;
                    dodged = false;
                }
            }
            if (!dodged)
            {
                if (hitCount == 1)
                    gl.logGameMessage("A pellet hits ", nameDefArticle, name);
                else
                    gl.logGameMessage(hitCount, " pellets strike ", nameDefArticle, name);
                handleDamage(gl, ak[0], dam);
                return true;
            }
            return false;
        }

        virtual public void damage(GameLogic gl, Attack ak)
        {
            if (isDead)
                return;

            int dam = doAttack(gl, ak);
            handleDamage(gl, ak, dam);
        }
    }
}
