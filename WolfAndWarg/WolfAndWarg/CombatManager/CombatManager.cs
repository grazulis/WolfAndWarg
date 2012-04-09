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
            //Sometimes the game incorrectly confuses lack of movement with an attack
            //This is a bodge fix until tile/map/combat sorted
            if (attacker == defender) return;

            if (rnd.NextDouble() > 0.4)
            {
                //If attack successful deduct 2 health
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
