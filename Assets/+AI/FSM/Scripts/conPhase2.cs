using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony
{
    [CreateAssetMenu(menuName = "AI/FSM/Conditions/Phase2")]
    public class conPhase2 : Condition
    {
        public override bool Test(FSM fsm)
        {
            if (fsm.bossController)
            {
                if (fsm.bossController.phase2Transition)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}