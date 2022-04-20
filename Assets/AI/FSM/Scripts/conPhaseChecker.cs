using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    [CreateAssetMenu(menuName = "AI/FSM/Conditions/PhaseChecker")]
    public class conPhaseChecker : Condition
    {
        public override bool Test(FSM fsm)
        {
            if (fsm.enemyController.currHealth <=  (fsm.enemyController.maxHealth / 2))
            {
                return true;
            }
            return false;
        }


    }

}
