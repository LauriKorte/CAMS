using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cougarsimulator2000
{
    public class Player : Actor
    {
        public ObservableCollection<Item> inventory = new ObservableCollection<Item>();
        public WeaponDefinition weapon = null;
        public Player()
        {

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
            return true;
        }
    }
}
