using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Theogony{
    [CreateAssetMenu(menuName = "AI/FSM/Actions/Patrol")]
    public class Patrol : Action
    {
        public override void Act(FSM fsm)
        {
            // if(fsm.GetNavMesh().isActiveAndEnabled)
            Debug.Log("Patrol");
        }
    }
}