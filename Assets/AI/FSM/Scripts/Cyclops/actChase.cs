using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Chase")]
    public class Chase : Action
    {
        public override void Act(FSM fsm){
            if(fsm.GetNavMesh().IsAtDestination()){
                fsm.GetNavMesh().GoToTarget();
            }
        }
    }
}