using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Face")]
    public class actFace : Action
    {
        public override void Startup(FSM fsm){
        }

        public override void Act(FSM fsm){
            Debug.Log("Facing");
            fsm.GetNavMesh().FaceTarget();
        }
    }
}