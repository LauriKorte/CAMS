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

        public override int attack(GameLogic gl, Actor ac)
        {
            Random r = new Random();
            int dam = 1 + r.Next(5);
            Attack ak = new Attack();
            ak.damage = dam;
            ak.accuracy = 12;
            ak.damageMessage = "is struck by the cougar's fierce claws";
            ak.dodgeMessage = "skilfully evades the fierce blow";

            ac.damage(gl,ak);
            return 14;
        }

        override public int update(GameLogic gl) 
        {

            if (gl.getLineOfSight(this, gl.player))
                spotTimer = 3;

            if (spotTimer > 0)
            {
                spotTimer--;
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
                        return moveSpeed;
                    }
                    else if (pfr.steps.Count == 2)
                    {
                        return attack(gl, gl.player);
                    }
                }
            }
            
            
            return 5;
        }
    }
}
