using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Stop")]
    public class actStop : Action
    {
        // public override void Start(FSM fsm){
            
        // }
        
        public override void Act(FSM fsm){
            Debug.Log("Stopping");
            fsm.GetNavMesh().Stop();
        }
    }
}