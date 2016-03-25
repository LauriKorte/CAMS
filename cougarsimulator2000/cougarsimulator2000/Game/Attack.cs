using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cougarsimulator2000
{
    struct Attack
    {
        public int damage;
        public int accuracyDice;
    }

    abstract class Weapon
    {
        abstract public Attack getAttack();
    }
}
