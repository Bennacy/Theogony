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
            if (fsm.bossController)
            {
              
                if (fsm.bossController.currHealth <= (fsm.bossController.maxHealth / 2))
                {
                   
                    return true;
                }
            }
            else if (fsm.enemyController)
            {
                if (fsm.enemyController.currHealth <= 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}



