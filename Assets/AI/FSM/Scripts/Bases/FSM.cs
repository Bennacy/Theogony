using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class FSM : MonoBehaviour
    {
        public State initialState;
        public State currentState;
        private MyNavMesh navMeshAgent;

        void Start(){
            currentState = initialState;
            navMeshAgent = GetComponent<MyNavMesh>();
        }

        void Update()
        {
            Transition triggeredTransition = null;
            foreach (Transition transition in currentState.GetTransitions()){
                if(transition.IsTriggered(this)){
                    triggeredTransition = transition;
                    break;
                }
            }
            List<Action> actions = new List<Action>();
            if(triggeredTransition){
                State targetState = triggeredTransition.GetTargetState();
                actions.Add(currentState.GetExitAction());
                actions.Add(triggeredTransition.GetAction());
                actions.Add(targetState.GetEntryAction());
                currentState = targetState;
            }else{
                foreach(Action a in currentState.GetActions()){
                    actions.Add(a);
                }
            }
            PerformActions(actions);
        }

        private void PerformActions(List<Action> actions){
            foreach(Action action in actions){
                if(action != null){
                    action.Act(this);
                }
            }
        }

        public MyNavMesh GetNavMesh(){
            return navMeshAgent;
        }
    }
}