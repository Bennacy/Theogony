using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gorgon/Finite State Machine/Action/Chase")]

public class ChaseAction : Action
{
    public override void Act(GorgonFiniteStateMachine fsm)
    {
        Debug.Log("Chase");
        fsm.GetAgent().GetComponent<MeshRenderer>().material.color = Color.green;
        fsm.GetAgent().GoToTarget();
        
    }
}
