using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gorgon/Finite State Machine/Action/Idle")]

public class IdleAction : Action
{
    public override void Act(GorgonFiniteStateMachine fsm)
    {
        Debug.Log("Idle");
    }
}
