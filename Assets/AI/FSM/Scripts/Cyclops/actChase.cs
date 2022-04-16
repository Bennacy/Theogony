using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Chase")]
    public class actChase : Action
    {
        public override void Act(FSM fsm){
            Debug.Log("Chasing");
            if(fsm.GetNavMesh().IsAtDestination()){
                fsm.GetNavMesh().GoToTarget();
            }
        }
    }
}