using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cougarsimulator2000
{
    public class Enemy : Actor
    {
        int spotTimer = 0;
        public Enemy()
        {
            this.isEnemy = true;
        }

        public override bool attack(GameLogic gl, Actor ac)
        {
            Random r = new Random();
            int dam = 1 + r.Next(5);
            ac.damage(gl,dam,"is struck by the cougar's fierce claws");
            return true;
        }

        override public void update(GameLogic gl) 
        {
            
            if (spotTimer >= 0)
            for (int i = 0; i < 2; i++) //Make cougars really spooky
            {
                //Cougars smell the fear of a man

                //That's why they beeline for you
                PathFindResult pfr = PathFinding.GetPath(
                    (Vector2 pos) =>
                    {
                        return gl.isTileBlockingNoActors(pos);
                    }
                    , position, gl.player.position);

                if (pfr.foundPath)
                {
                    if (pfr.steps.Count > 2)
                    {
                        if (!gl.isTileBlocking(pfr.steps[1]))
                            position = pfr.steps[1];
                    }
                    else if (pfr.steps.Count == 2)
                    {
                        attack(gl, gl.player);
                    }
                }
            }
            
            if (gl.getLineOfSight(this, gl.player))
                spotTimer = 3;
            spotTimer--;
        }
    }
}
