using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Patrol")]
    public class actPatrol : Action
    {
        public Vector3[] waypoints;
        private Vector3 currentTarget;
        private int waypointIndex;
        private EnemyController enemyController;
        public override void Startup(FSM fsm)
        {
            fsm.changedState = false;
            enemyController = fsm.gameObject.GetComponent<EnemyController>();
            waypoints = enemyController.patrolWaypoints;
            GetClosestWaypoint(fsm);
            currentTarget = waypoints[waypointIndex];
        }
        public override void Act(FSM fsm)
        {
            if(fsm.changedState){
                Startup(fsm);
            }
            if(fsm.GetNavMesh().IsAtDestination()){
                NextWaypoint(fsm);
            }
        }

        private void NextWaypoint(FSM fsm){
            waypointIndex++;
            if(waypointIndex >= waypoints.Length){
                waypointIndex = 0;
            }
            enemyController.waypointIndex = waypointIndex;
            currentTarget = waypoints[waypointIndex];
            fsm.GetNavMesh().SetTarget(currentTarget);
        }

        private void GetClosestWaypoint(FSM fsm){
            float distance = float.PositiveInfinity;
            for(int i = 0; i < waypoints.Length; i++){
                Debug.Log(waypoints[i]);
                if(Vector3.Distance(fsm.transform.position, waypoints[i]) < distance){
                    distance = Vector3.Distance(fsm.transform.position, waypoints[i]);
                    waypointIndex = i;
                }
            }
            fsm.GetNavMesh().SetTarget(waypoints[waypointIndex]);
        }
    }
}