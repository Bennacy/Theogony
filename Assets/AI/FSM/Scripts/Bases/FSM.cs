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
        public BossController bossController;
        public float staggerTimer;
        public Vector2 attackTracker; //X value is the attack index, Y is the number of times the attack was performed
        void Start(){
            currentState = initialState;
            enemyController = GetComponent<EnemyController>();
            bossController = GetComponent<BossController>();
            navMeshAgent = GetComponent<MyNavMesh>();
        }

        void Update()
        {
            if (bossController)
            {
                GetNavMesh().FaceTarget();
            }
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
            if (enemyController)
            {
                if (!enemyController.attacking || enemyController.currHealth <= 0 || enemyController.staggered)
                {
                    PerformActions(actions);
                }
            }else if (bossController)
            {
                if (!bossController.attacking || bossController.currHealth <= 0 || bossController.staggered)
                {
                    PerformActions(actions);
                }
            }
        }

        private void PerformActions(List<Action> actions){
            foreach(Action action in actions){
                if(changedState && action != null){
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