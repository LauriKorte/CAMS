using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cougarsimulator2000
{
    public class Player : Actor
    {
        public ObservableCollection<Item> inventory
        {
            get;
            set;
        }
        public WeaponDefinition weapon = null;



        public Item getItemByName(string name)
        {
            foreach(var v in inventory)
            {
                if (v.definition.name == name)
                    return v;
            }
            return null;
        }
        public Item getItemByDef(ItemDefinition idef)
        {
            foreach (var v in inventory)
            {
                if (v.definition == idef)
                    return v;
            }
            return null;
        }

        public void changeItemCount(Item i, int change)
        {
            i.count += change;
            if (i.count <= 0)
            {
                inventory.Remove(i);
            }
        }
        public void addItem(Item i)
        {
            Item old = getItemByDef(i.definition);
            if (old == null)
            {
                inventory.Add(i);
            }
            else
            {
                old.count += i.count;
            }
        }

        public Player()
        {
            health = 25;
            inventory = new ObservableCollection<Item>();
        }

        override public void update(GameLogic gl)
        {
        }

        public override bool attack(GameLogic gl, Actor ac)
        {
            if (weapon == null)
            {
                gl.logGameMessage("Ya got no wappet son!");
                return false;
            }
            if (weapon.ammunitionDefinition != null)
            {
                Item it = getItemByDef(weapon.ammunitionDefinition);
                if (it == null)
                {
                    gl.logGameMessage("Ya got no ammo son!");
                    return false;
                }
                changeItemCount(it, -1);
            }

            //We calculate the base weapon accuracy
            //Every Actor (including the player)
            //has a dodge stat, which influences the accuracy

            //the final accuracy is base_accuracy - target.dodge

            //Greater accuracy means greater chance to hit:
            //if the final accuracy is greater than 15, miss is impossible
            //if it's less than 1, miss is unavoidable

            //Here our accuracy algorithm is basically the following:
            //If distance in tiles is less than 2 then
            //  accuracy = weapon.accuracy
            //else
            //  accuracy = weapon.accuracy - (distance - 2) / 2

            int accuracy = weapon.accuracy;
            int distance = (int)Math.Sqrt(Math.Pow(position.x - ac.position.x,2) + Math.Pow(position.y - ac.position.y, 2));
            if (distance >= 2)
                accuracy -= (distance - 2) / 2;
            
            //Add the weapon dice up
            int dam = weapon.damageBonus;
            Random r = new Random();
            for (int i = 0; i < weapon.damageDieCount; i++)
            {
                dam += r.Next(weapon.damageDieSize)+1;
            }

            Attack ak = new Attack();
            ak.damage = dam;
            ak.accuracy = accuracy;

            //If the player is shooting themselves
            if (ac == this)
            {
                ak.damageMessage = "took the cowards way out";
                ak.dodgeMessage = "dodges the bullet. Maybe he isn't ready to leave this world.";
            }
            else
            {
                ak.damageMessage = "was shot";
                ak.dodgeMessage = "dodges the bullet";
            }
            ac.damage(gl, ak);
            return true;
        }
    }
}
