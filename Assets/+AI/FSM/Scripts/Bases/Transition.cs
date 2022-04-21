using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Transition")]
    public class Transition : ScriptableObject
    {
        [SerializeField]  private Theogony.Condition decision;
        [SerializeField]  private Theogony.Action action;
        [SerializeField] private Theogony.State targetState;

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