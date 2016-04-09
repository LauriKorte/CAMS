using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cougarsimulator2000
{
    class Prop : Actor
    {

        public Prop(string propName)
        {
            image = propName;
            Random r = new Random();
            imageAngle = r.NextDouble() * 360.0;
            dodge = 612;
            depth = -1;
            isBlocking = false;
            isEnemy = false;
        }
        public override void damage(GameLogic gl, Attack ak)
        {
            handleDamage(gl, ak, 0);
        }
        public override bool damageMultiple(GameLogic gl, List<Attack> ak)
        {
            handleDamage(gl, null, 0);
            return false;
        }
        protected override void handleDamage(GameLogic gl, Attack ak, int dam)
        {
            gl.removeActor(this);
            return;
        }

    }
}
