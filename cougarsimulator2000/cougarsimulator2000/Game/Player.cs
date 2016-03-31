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

        public Player()
        {
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
            gl.logGameMessage("Ya got a wappet son!");
            if (weapon.ammunitionDefinition != null)
            {
                Item it = getItemByDef(weapon.ammunitionDefinition);
                if (it == null)
                {
                    gl.logGameMessage("Ya got no ammo son!");
                    return false;
                }
                gl.logGameMessage("Ya shot some of them ", it.definition.name);
                changeItemCount(it, -1);
                gl.logGameMessage("Ya got only ", it.count, " left.");
            }
            gl.logGameMessage("");
            int dam = weapon.damageBonus;
            Random r = new Random();
            for (int i = 0; i < weapon.damageDieCount; i++)
            {
                dam += r.Next(weapon.damageDieSize);
            }
            ac.damage(gl, dam, "was shot a bit");
            
            return true;
        }
    }
}
