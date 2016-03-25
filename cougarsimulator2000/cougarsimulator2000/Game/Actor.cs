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
        public int depth = 0;
        public string image;

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

        virtual public void Update(GameLogic gl)
        {
        }

    }
}
