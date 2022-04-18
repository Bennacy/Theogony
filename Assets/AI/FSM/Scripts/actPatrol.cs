using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Patrol")]
    public class actPatrol : Action
    {
        public Vector3[] waypoints;
        public float distanceThreshold;
        private Vector3 currentTarget;
        private int waypointIndex;
        private EnemyController enemyController;
        public override void Startup(FSM fsm)
        {
            enemyController = fsm.gameObject.GetComponent<EnemyController>();
            waypoints = enemyController.patrolWaypoints;
            GetClosestWaypoint(fsm);
            fsm.GetNavMesh().agent.stoppingDistance = distanceThreshold;
            if(waypoints.Length > 0){
                currentTarget = waypoints[waypointIndex];
            }
        }
        public override void Act(FSM fsm)
        {
            if(fsm.GetNavMesh().IsAtDestination()){
                NextWaypoint(fsm);
            }
        }

        private void NextWaypoint(FSM fsm){
            if(waypoints.Length > 0){
                waypointIndex++;
                if(waypointIndex >= waypoints.Length){
                    waypointIndex = 0;
                }
                enemyController.waypointIndex = waypointIndex;
                currentTarget = waypoints[waypointIndex];
                fsm.GetNavMesh().SetTarget(currentTarget);
            }
        }

        private void GetClosestWaypoint(FSM fsm){
            float distance = float.PositiveInfinity;
            if(waypoints.Length > 0){
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
}