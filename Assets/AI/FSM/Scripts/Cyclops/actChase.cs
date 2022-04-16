using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Chase")]
    public class actChase : Action
    {
        public override void Startup(FSM fsm){
            if(fsm.changedState){
                fsm.changedState = false;
                fsm.GetNavMesh().GoToTarget();
            }
        }

        public override void Act(FSM fsm){
            if(fsm.GetNavMesh().IsAtDestination()){
                fsm.GetNavMesh().GoToTarget();
            }
        }
    }
}