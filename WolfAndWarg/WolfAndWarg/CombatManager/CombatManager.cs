using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WolfAndWarg.Game;

namespace WolfAndWarg
{
    class CombatManager
    {
        //Probably make this non-static in future.
        public static void Attack(ISprite attacker, ISprite defender)
        {
            //If attack successful deduct 2 health
            if (rnd.NextDouble() > 0.4)
            {
                defender.Health -= 2;

            }
            else
            {
                //Otherwise attacker loses 1 health in counter-attack
                attacker.Health--;
            }
        }

        static Random rnd = new Random(DateTime.Now.Millisecond);
    }
}
