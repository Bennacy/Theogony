using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gorgon/Finite State Machine/Action/Die")]

public class DieAction : Action
{
    public override void Act(GorgonFiniteStateMachine fsm)
    {
        Debug.Log("Die");
        // fsm.GetAgent().GetComponent<MeshRenderer>().material.color = Color.white;
        //distroy enemy
    }
}
