using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gorgon/Finite State Machine/Condition/DieCondition")]
public class DieCondition : Condition
{
    public override bool Test(GorgonFiniteStateMachine fsm)
    {
    //    if(fsm.GetAgent().GetHealth() == 0)
    //     {
    //         return true;
    //     }
        return false;
    }
}
