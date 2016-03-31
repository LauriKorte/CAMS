using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cougarsimulator2000
{
    public class Actor
    {
        public Vector2 position;
        public int health = 10;
        public int depth = 0;
        public bool isDead = false;
        public string image;

        public string nameArticle = "a ";
        public string nameDefArticle = "the ";
        public string name = "cougar";
        public string postMortem = "da ding is dead";
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
