using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cyclops/Finite State Machine/Action/IsHit")]
public class IsHitAction : Action
{
    public override void Act(GorgonFiniteStateMachine fsm)
    {
        Debug.Log("Health: "+ fsm.GetAgent().GetHealth());
        fsm.animator.Play("IsHit");
    }
}
