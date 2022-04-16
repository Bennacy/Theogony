using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/State")]
    public class State : ScriptableObject
    {
        [SerializeField] private Action entryAction;
        [SerializeField] private Action[] stateActions;
        [SerializeField]  private Action exitAction;
        [SerializeField] private Transition[] transitions;

        public Action[] GetActions(){
            return stateActions;
        }

        public Action GetEntryAction(){
            return entryAction;
        }

        public Action GetExitAction(){
            return exitAction;
        }

        public Transition[] GetTransitions(){
            return transitions;
        }
    }
}