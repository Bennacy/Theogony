using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gorgon/Finite State Machine/Action/Stop")]

public class StopAction : Action
{
    public override void Act(GorgonFiniteStateMachine fsm)
    {
        // throw new System.NotImplementedException();
        Debug.Log("Stop");
        fsm.GetAgent().StopAction();
    }
}
