using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Transition")]
    public abstract class Transition : ScriptableObject
    {
        [SerializeField]  private Condition decision;
        [SerializeField]  private Action action;
        [SerializeField] private State targetState;

        public bool IsTriggered(FSM fsm){
            return decision.Test(fsm);
        }

        public State GetTargetState(){
            return targetState;
        }

        public Action GetAction(){
            return action;
        }
    }
}