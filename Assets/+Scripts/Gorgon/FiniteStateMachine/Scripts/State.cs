using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Gorgon/Finite State Machine/State")]
public class State : ScriptableObject
{
    [SerializeField] private Action entryAction;
    [SerializeField] private Action exitAction;
    [SerializeField] private Action[] stateAction;
    [SerializeField] private Transition[] transitions;

    public Action[] GetStateActions()
    {
        return stateAction;
    }
    public Action GetEntryAction()
    {
        return entryAction;
    }
    public Action GetExitAction()
    {
        return exitAction;
    }
    public Transition[] GetTransitions()
    {
        return transitions;
    }
}
