using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            maxHealth = 25;
            armorRating = 6;
            inventory = new ObservableCollection<Item>();
        }

        override public int update(GameLogic gl)
        {
            return 10;
        }

        public override int attack(GameLogic gl, Actor ac)
        {
            if (weapon == null)
            {
                gl.logGameMessage("Ya got no wappet son!");
                return 0;
            }

            //Chebyshev distance is used for range check
            int cbdist = Math.Max(Math.Abs(position.x - ac.position.x),Math.Abs(position.y - ac.position.y));
            if (cbdist > weapon.range)
            {
                gl.logGameMessage("Your target is too far away!");
                return 0;
            }
            if (weapon.ammunitionDefinition != null)
            {
                Item it = getItemByDef(weapon.ammunitionDefinition);
                if (it == null)
                {
                    gl.logGameMessage("Ya got no ammo son!");
                    return 0;
                }
                changeItemCount(it, -1);
            }
            Random r = new Random();



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
            int distance = (int)Math.Sqrt(Math.Pow(position.x - ac.position.x, 2) + Math.Pow(position.y - ac.position.y, 2));
            if (distance >= 2)
                accuracy -= (distance - 2) / 2;

            //Fire the wappet
            gl.playSound(weapon.fireSound);

            //If we have weapon using pelleted firing algorithm
            if (weapon.pellets > 0)
            {
                accuracy += 8;
                Vector2 delta = ac.position - position;
                double angle = Math.Atan2(delta.y, delta.x) * 180.0 / Math.PI;
                double spread = weapon.pelletSpread;
                int pelletCount = weapon.pellets;

                //We bunch up all the hit pellets
                //so we can avoid filling the game log with attack messages
                var dict = new Dictionary<Actor, List<Attack>>();
                for (int i = 0; i < pelletCount; i++)
                {
                    double ang = angle - spread / 2 + r.NextDouble() * spread;
                    var actors = gl.castRay(position, ang);

                    foreach (Actor v in actors)
                    {
                        //In case the player isn't trying to kill themselves
                        //We assume the PC is skillful enough to not shoot 
                        //themselves
                        if (this == v && ac != this)
                            continue;
                        //Check if the hit target is beyond the range
                        int chbdist = Math.Max(Math.Abs(position.x - v.position.x), Math.Abs(position.y - v.position.y));
                        if (chbdist > weapon.range)
                            break;

                        
                        //Each pellet has chance to miss the first target
                        bool dodgeCheck = false;
                        for (int d = 0; d < v.dodge; d++)
                        {
                            if (r.Next(10) == 0)
                            {
                                dodgeCheck = true;
                                break;
                            }
                        }

                        if (dodgeCheck)
                            continue;

                        //Add the weapon dice up
                        int dammage = weapon.damageBonus;
                        for (int i2 = 0; i2 < weapon.damageDieCount; i2++)
                        {
                            dammage += r.Next(weapon.damageDieSize) + 1;
                        }

                        Attack atk = new Attack();
                        atk.damage = dammage;
                        atk.accuracy = accuracy;

                        atk.damageMessage = "was hit";
                        atk.dodgeMessage = "dodges the attack";
                        if (!dict.ContainsKey(v))
                            dict[v] = new List<Attack>();
                        dict[v].Add(atk);

                        //Random pellet penetration
                        if (r.Next(4) == 0)
                            continue;
                        break;
                    }
                }
                bool anyHit = false;
                foreach (var v in dict)
                {
                    anyHit = v.Key.damageMultiple(gl, v.Value) || anyHit;
                }
                if (!anyHit)
                {
                    gl.logGameMessage("You miss");
                }
            }
            else
            {
                //Single shot weapon
                
                //Add the weapon dice up
                int dam = weapon.damageBonus;
                for (int i = 0; i < weapon.damageDieCount; i++)
                {
                    dam += r.Next(weapon.damageDieSize) + 1;
                }

                Attack ak = new Attack();
                ak.damage = dam;
                ak.accuracy = accuracy;

                //If the player is shooting themselves
                if (ac == this)
                {
                    ak.damageMessage = "hits himself";
                    ak.dodgeMessage = "dodges his own attack. Maybe he isn't ready to leave this world.";
                }
                else
                {
                    ak.damageMessage = "was hit";
                    ak.dodgeMessage = "dodges the attack";
                }
                ac.damage(gl, ak);
            }
            return weapon.fireSpeed;
        }

        public void use(GameLogic gl, Item selitem)
        {
            if (selitem.definition.itemType != ItemType.Consumable)
            {
                gl.logGameMessage("Ya can't use that!");
            }
            else
            {
                ConsumableDefinition idef = selitem.definition as ConsumableDefinition;
                changeItemCount(selitem, -1);

                if (idef.effect == "healing")
                {
                    int healAmount = idef.amount;
                    if (healAmount + health >= maxHealth)
                        healAmount = maxHealth - health;
                    if (healAmount == 0)
                        gl.logGameMessage("You used the medkit, but with no effect.");
                    else
                        gl.logGameMessage("You used the medkit, and gained ",healAmount," HP");

                    health += healAmount;
                }

            }
        }

        public void equip(GameLogic gl, Item selitem)
        {
            if (selitem.definition.itemType != ItemType.Weapon)
            {
                gl.logGameMessage("Ya can't equip that!");
            }
            else
            {
                gl.logGameMessage("Wielding ", selitem.definition.name);
                weapon = selitem.definition as WeaponDefinition;
            } 
        }

        override public void onDeath(GameLogic gl)
        {
            gl.addNewScore(gl.score);
        }
    }
}
