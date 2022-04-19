using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Face")]
    public class actFace : Action
    {
        public override void Startup(FSM fsm){
            fsm.GetNavMesh().GoToTarget();
        }

        public override void Act(FSM fsm){
            fsm.GetNavMesh().FaceTarget();
        }
    }
}