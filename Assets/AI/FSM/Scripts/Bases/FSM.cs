using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    public class FSM : MonoBehaviour
    {
        public State initialState;
        public State currentState;
        public bool changedState;
        private MyNavMesh navMeshAgent;
        public EnemyController enemyController;
        public Vector2 attackTracker; //X value is the attack index, Y is the number of times the attack was performed
        void Start(){
            currentState = initialState;
            enemyController = GetComponent<EnemyController>();
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
                changedState = true;
                actions.Add(currentState.GetExitAction());
                actions.Add(triggeredTransition.GetAction());
                actions.Add(targetState.GetEntryAction());
                currentState = targetState;
            }else{
                foreach(Action a in currentState.GetActions()){
                    actions.Add(a);
                }
            }
            if(!enemyController.attacking || enemyController.currHealth < 0){
                PerformActions(actions);
            }
        }

        private void PerformActions(List<Action> actions){
            foreach(Action action in actions){
                if(changedState && action != null){
                    Debug.Log("A");
                    action.Startup(this);
                }
                if(action != null){
                    action.Act(this);
                }
            }
            changedState = false;
        }

        public MyNavMesh GetNavMesh(){
            return navMeshAgent;
        }
    }
}