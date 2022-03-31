using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GorgonFiniteStateMachine : MonoBehaviour
{
    public State initialState;
    public State currentState;
    private MyNavMesh agent;
    public Animator gorgonAnimator;
    public float cooldowntimer = 1f;
    void Start()
    {
        gorgonAnimator = GetComponentInChildren<Animator>();
        currentState = initialState;
        agent = GetComponent<MyNavMesh>();

    }
    public MyNavMesh GetAgent()
    {
        return agent;
    }


    void Update()
    {
        
        Transition triggeredTransition = null;
        foreach (Transition transition in currentState.GetTransitions())
        {
            if (transition.isTRiggered(this))
            {
                triggeredTransition = transition;
                break;
            }
        }
        List<Action> actions = new List<Action>();
        if (triggeredTransition)
        {
            State targetState = triggeredTransition.GetTargetState();
            actions.Add(currentState.GetExitAction());
            actions.Add(triggeredTransition.GetAction());
            actions.Add(targetState.GetExitAction());
            currentState = targetState;
        }
        else
        {
            foreach (Action action in currentState.GetStateActions())
            {
                actions.Add(action);
            }
            foreach (Action action in actions)
            {
                if (action)
                {
                    cooldowntimer = 1f;
                    action.Act(this);
                }
            }
        }
    }
   
}
