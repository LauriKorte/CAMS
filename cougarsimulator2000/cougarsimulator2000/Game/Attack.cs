using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cougarsimulator2000
{
    public class Attack
    {
        //Damage to deal
        public int damage;

        //Shot accuracy before enemy dodge subtraction
        //Greater accuracy means greater chance to hit
        //After that, accuracy of 7 has about 50% chance to hit

        public int accuracy;

        //Message that will be displayed in form
        //(enemy name) (damageMessage) for (x) damage
        //The cougar   got shot       for 251 damage
        public string damageMessage;

        //Message that will be displayed upon a dodge
        //(enemy name) (dodgeMessage)
        //The cougar   dodged the bullet
        public string dodgeMessage;
    }
}
