using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gorgon/Finite State Machine/Condition/FinishAnimationCondition")]

public class FinishAnimationCondition : Condition
{
    public override bool Test(GorgonFiniteStateMachine fsm)
    {
        //back to Chase after the Hit Animation is done
        return false;
    }
}
