using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gorgon/Finite State Machine/Transition")]

public class Transition : ScriptableObject
{

    [SerializeField] private Condition decision;
    [SerializeField] private Action action;
    [SerializeField] private State targetState;
    public bool isTRiggered(GorgonFiniteStateMachine fsm)
    {
        return decision.Test(fsm);
    }
    public State GetTargetState()
    {
        return targetState;
    }
    public Action GetAction()
    {
        return action;
    }
}
