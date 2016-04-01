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
            item = it;
            image = it.definition.icon;
            health = 5;
            postMortem = "The " + it.definition.name + " is destroyed.";
            goryPostMortem = postMortem;
            name = it.definition.name;
            depth = 0;
            isBlocking = false;
        }

        public override void update(GameLogic gl)
        {
            if (gl.player.position == position)
            {

                if (item.count == 1)
                    gl.logGameMessage("Picked up one ", item.definition.name);
                else
                    gl.logGameMessage("Picked up ",item.count," pieces of ", item.definition.name);
                gl.player.addItem(item);
                gl.removeActor(this);
            }
        }
    }
}
