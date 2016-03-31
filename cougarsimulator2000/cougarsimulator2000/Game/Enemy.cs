using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cougarsimulator2000
{
    public class Enemy : Actor
    {
        public Enemy()
        {
            this.isEnemy = true;
        }

        override public void update(GameLogic gl) 
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
                if (pfr.steps.Count > 1)
                if (!gl.isTileBlocking(pfr.steps[1]))
                    position = pfr.steps[1];
            }
        }
    }
}
