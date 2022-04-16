using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Patrol")]
    public class actPatrol : Action
    {
        public Vector3[] waypoints;
        // public override void Start(FSM fsm)
        // {
        //     EnemyController enemyController = fsm.gameObject.GetComponent<EnemyController>();
        //     waypoints = enemyController.patrolWaypoints;
        // }
        public override void Act(FSM fsm)
        {
            // if(fsm.GetNavMesh().isActiveAndEnabled)
            Debug.Log("Patrol");
        }
    }
}