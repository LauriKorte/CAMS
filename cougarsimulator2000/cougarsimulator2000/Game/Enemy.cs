using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cougarsimulator2000
{
    public class Enemy : Actor
    {
        EnemyDefinition definition;

        int spotTimer = 0;
        public Enemy(EnemyDefinition def)
        {
            definition = def;
            this.isEnemy = true;

            dodge = def.dodge;
            postMortem = def.postMortem;
            goryPostMortem = def.goryPostMortem;
            image = def.image;

            name = def.name;
            nameArticle = def.nameArticle;
            nameDefArticle = def.nameDefArticle;

            moveSpeed = def.moveSpeed;
            health = def.health;

            depth = 1;
        }

        public override int attack(GameLogic gl, Actor ac)
        {
            Random r = new Random();
            Attack ak = new Attack();

            int damage = definition.damageBonus;
            for (int i = 0; i < definition.damageDieCount; i++)
                damage += r.Next(definition.damageDieSize) + 1;

            ak.damage = damage;
            ak.accuracy = definition.attackAccuracy;
            ak.damageMessage = definition.attackMessage;
            ak.dodgeMessage = definition.attackDodgeMessage;

            ac.damage(gl,ak);
            return definition.attackSpeed;
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
