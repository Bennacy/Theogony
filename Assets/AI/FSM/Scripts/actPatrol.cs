using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Patrol")]
    public class actPatrol : Action
    {
        public float distanceThreshold;
        private Vector3 currentTarget;
        private int waypointIndex;
        private EnemyController enemyController;
        public override void Startup(FSM fsm)
        {
            enemyController = fsm.enemyController;
            GetClosestWaypoint(fsm);
            fsm.GetNavMesh().agent.stoppingDistance = distanceThreshold;

            if(enemyController.patrolWaypoints.Length > 0){
                currentTarget = enemyController.patrolWaypoints[waypointIndex];
            }
        }
        public override void Act(FSM fsm)
        {
            // enemyController = fsm.enemyController;
            if(fsm.GetNavMesh().IsAtDestination()){
                NextWaypoint(fsm);
            }
        }

        private void NextWaypoint(FSM fsm){
            if(enemyController.patrolWaypoints.Length > 0){
                waypointIndex++;
                if(waypointIndex >= enemyController.patrolWaypoints.Length){
                    waypointIndex = 0;
                }
                enemyController.waypointIndex = waypointIndex;
                currentTarget = enemyController.patrolWaypoints[waypointIndex];
                fsm.GetNavMesh().SetTarget(currentTarget);
            }
        }

        private void GetClosestWaypoint(FSM fsm){
            float distance = float.PositiveInfinity;
            if(enemyController.patrolWaypoints.Length > 0){
                for(int i = 0; i < enemyController.patrolWaypoints.Length; i++){
                    if(Vector3.Distance(fsm.transform.position, enemyController.patrolWaypoints[i]) < distance){
                        distance = Vector3.Distance(fsm.transform.position, enemyController.patrolWaypoints[i]);
                        waypointIndex = i;
                    }
                }
                fsm.GetNavMesh().SetTarget(enemyController.patrolWaypoints[waypointIndex]);
            }
        }
    }
}