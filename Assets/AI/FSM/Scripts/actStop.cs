using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Stop")]
    public class actStop : Action
    {
        public override void Startup(FSM fsm){
            
        }
        
        public override void Act(FSM fsm){
            fsm.GetNavMesh().Stop();
        }
    }
}