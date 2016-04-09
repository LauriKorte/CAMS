using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cougarsimulator2000
{
    public class PickUp : Actor
    {
        private Item item;
        
        public PickUp(Item it)
        {
            //Give pickups some dodge skill
            dodge = 12;
            item = it;
            image = it.definition.icon;
            health = 5;
            nameArticle = "";
            nameDefArticle = "the ";
            postMortem = "The " + it.definition.name + " is destroyed.";
            goryPostMortem = postMortem;
            name = it.definition.name;
            depth = 0;
            isBlocking = false;
        }

        protected override void spawnBlood(GameLogic gl)
        {
            //We don't spawn blood for pickups
            return;
        }

        public override int update(GameLogic gl)
        {
            if (gl.player.position == position)
            {
                gl.playSound("pickup");
                if (item.count == 1)
                    gl.logGameMessage("Picked up one ", item.definition.name);
                else
                    gl.logGameMessage("Picked up ",item.count," pieces of ", item.definition.name);
                gl.player.addItem(item);
                gl.removeActor(this);
            }
            return 1; 
        }
    }
}
